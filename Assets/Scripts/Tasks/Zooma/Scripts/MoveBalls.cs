using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using BansheeGz.BGSpline.Components;
using BansheeGz.BGSpline.Curve;
using DG.Tweening;
using System;

public struct ActiveBallList
{
    List<GameObject> ballList;
    bool isMoving;
    bool isInTransition;
}

public enum BallColor
{
    red,
    green,
}

public class MoveBalls : MonoBehaviour
{
    public GameObject ball;

    public float pathSpeed;
    public float mergeSpeed;
    public int ballCount;

    public Ease easeType;
    public Ease mergeEaseType;

    // Private
    private List<GameObject> ballList;
    private GameObject ballsContainerGO;
    private GameObject removedBallsContainer;
    private PlayerDataUI playerDataUI;
    private BGCurve bgCurve;
    private float distance = 0;
    private int headballIndex;
    private int HeadballIndex
    {
        get { return headballIndex; }
        set { 
            if (value < 0) {
                headballIndex = 0;
                return;
            }
            else {
                headballIndex  = value;
            } 
        }
    }


    private SectionData sectionData;
    [SerializeField]
    private int addBallIndex;
    private int touchedBallIndex;
    private float ballRadius;

    private bool oneTimeWictory = false;

    // Use this for initialization
    private void Start()
    {
        playerDataUI = GameObject.FindAnyObjectByType<PlayerDataUI>();
        ballRadius = ball.transform.localScale.x;
        //ballRadius = redBall.transform.localScale.x;
        HeadballIndex = 0;
        addBallIndex = -1;

        bgCurve = GetComponent<BGCurve>();
        ballList = new List<GameObject>();

        ballsContainerGO = new GameObject();
        ballsContainerGO.name = "Balls Container";

        removedBallsContainer = new GameObject();
        removedBallsContainer.name = "Removed Balls Container";

        for (int i = 0; i < ballCount; i++)
            CreateNewBall();

        sectionData = new SectionData();
    }

    // Update is called once per frame
    private void Update()
    {
        if (sectionData.ballSections.Count > 1 && addBallIndex != -1 && addBallIndex < HeadballIndex)
            MoveStopeedBallsAlongPath();

        if (ballList.Count > 0)
            MoveAllBallsAlongPath();


        if (HeadballIndex != 0 && HeadballIndex != -1)
            JoinStoppedSections(HeadballIndex, HeadballIndex - 1);

        if (addBallIndex != -1)
            AddNewBallAndDeleteMatched();

        if (CheckIfActiveEndsMatch())
            MergeActiveEnds();

        MergeIfStoppedEndsMatch();

        if(ballsContainerGO.gameObject.transform.childCount == 0 && !oneTimeWictory)
        {
            oneTimeWictory = true;

            int TaskID = SceneManager.GetActiveScene().buildIndex;
            DateTime EndTime = DateTime.Now;
            SQLiteDbConnection Conn = new SQLiteDbConnection();
            Conn.executeSetQuery("UPDATE Task SET EndTime ='" + EndTime.ToLongTimeString() + "' WHERE Id=" + TaskID);

            string query = "SELECT * FROM Task WHERE Id =" + TaskID;
            List<Dictionary<string, object>> result = Conn.executeGetQuery(query);

            DateTime StartTime = DateTime.Parse(result[0]["StartTime"].ToString());
            TimeSpan score = EndTime - StartTime;
            int totalScore = score.Seconds + score.Minutes * 60;

            Conn.executeSetQuery("UPDATE Task SET Result ='" + totalScore + "' WHERE Id=" + TaskID);

            int Result = Convert.ToInt32(result[0]["PaySize"]);
            if (totalScore >= 90)
            { Result -= (Result * totalScore / 90) / 100; }

            playerDataUI.victoryUI.onpenVictoryUI(Result);
        }
    }

    /*
	 * Public Section
	 * =============
	 */
    public void AddNewBallAt(GameObject go, int index, int touchedBallIdx)
    {
       
        addBallIndex = index < 0 ? 0 : index;
        touchedBallIndex = touchedBallIdx;

        if (index > ballList.Count)
            ballList.Add(go);
        else
            ballList.Insert(index, go);

        go.transform.parent = ballsContainerGO.transform;
        go.transform.SetSiblingIndex(index);

        if (touchedBallIdx < HeadballIndex)
            HeadballIndex++;

        sectionData.OnAddModifySections(touchedBallIdx);

        // adjust distance for headBall or the position of the front in the added section
        PushSectionForwardByUnit();
    }

    /*
	 * Static Section
	 * =============
	 */

    public static BallColor GetRandomBallColor()
    {
        int rInt = UnityEngine.Random.Range(0, 2);
        return (BallColor)rInt;
    }

    /*
	 * Private Section
	 * =============
	 */

    private void CreateNewBall()
    {
      
        switch (GetRandomBallColor())
        {
            case BallColor.red:
                //InstatiateBall(redBall);
                InstatiateBall(ball, 0);
                
                break;

            case BallColor.green:
                InstatiateBall(ball, 1);
                //InstatiateBall(greenBall);
                break;
        }
    }

    private void InstatiateBall(GameObject ballGameObject, int type)
    {
        GameObject go = Instantiate(ballGameObject, bgCurve[0].PositionWorld, Quaternion.identity, ballsContainerGO.transform);
        go.SetActive(false);
        ballList.Add(go.gameObject);

        Ball instanceBallComponent = go.GetComponentInChildren<Ball>();
        if (type == 0)
        {
            string[] letters = new string[] { "Б", "В", "Г", "Д", "З", "К", "Л", "М", "Н", "П", "Р", "С", "Т", "Ф", "Х" };
            int randomIndex = UnityEngine.Random.Range(0, letters.Length);

            instanceBallComponent.Initialise(type, letters[randomIndex]);
        }
        else
        {
 
            string[] letters = new string[] { "Ж", "Ц", "Ч", "Ш", "Щ" };
            int randomIndex = UnityEngine.Random.Range(0, letters.Length);
            instanceBallComponent.Initialise(type, letters[randomIndex]);
        }
        
    }

    // When a new Ball is added to the one of the stopped sections move the balls to their correct positions
    private void MoveStopeedBallsAlongPath()
    {
        int sectionKey = sectionData.GetSectionKey(addBallIndex);
        int sectionKeyVal = sectionData.ballSections[sectionKey];
        int movingBallCount = 1;

        float sectionHeadDist;
        GetComponent<BGCcMath>().CalcPositionByClosestPoint(ballList[sectionKeyVal].transform.position, out sectionHeadDist);

        Vector3 tangent;
        Vector3 trailPos;
        float currentBallDist;
        for (int i = sectionKeyVal + 1; i <= sectionKey; i++)
        {
            currentBallDist = sectionHeadDist - movingBallCount * ballRadius;
            trailPos = GetComponent<BGCcMath>().CalcPositionAndTangentByDistance(currentBallDist, out tangent);

            if (i == addBallIndex && addBallIndex != -1)
                ballList[i].transform.DOMove(trailPos, 0.5f)
                    .SetEase(easeType);
            else
                ballList[i].transform.DOMove(trailPos, 1);

            movingBallCount++;
        }
    }

    // Move the active section of balls along the path
    private void MoveAllBallsAlongPath()
    {
        Vector3 tangent;
        int movingBallCount = 1;
        distance += pathSpeed * Time.deltaTime;

        // use a head index value which leads the balls on the path
        // This value will be changed when balls are delected from the path
        Vector3 headPos = GetComponent<BGCcMath>().CalcPositionAndTangentByDistance(distance, out tangent);
        ballList[HeadballIndex].transform.DOMove(headPos, 1);
        ballList[HeadballIndex].transform.rotation = Quaternion.LookRotation(tangent);

        if (!ballList[HeadballIndex].activeSelf)
            ballList[HeadballIndex].SetActive(true);

        for (int i = HeadballIndex + 1; i < ballList.Count; i++)
        {
            float currentBallDist = distance - movingBallCount * ballRadius;
            Vector3 trailPos = GetComponent<BGCcMath>().CalcPositionAndTangentByDistance(currentBallDist, out tangent);

            if (i == addBallIndex && addBallIndex != -1)
                ballList[i].transform.DOMove(trailPos, 0.5f)
                    .SetEase(easeType);
            else
                ballList[i].transform.DOMove(trailPos, 1);

            ballList[i].transform.rotation = Quaternion.LookRotation(tangent);

            if (!ballList[i].activeSelf)
                ballList[i].SetActive(true);

            movingBallCount++;
        }
    }

    // When using a different tween speed, there has to be a reset to the tween speed when the added balls reaches it correct position
    private bool CheckIfPushNeeded()
    {
        if (addBallIndex != ballList.Count)
        {
            Vector3 ABallPos = ballList[addBallIndex].transform.position;

            int neighbourBall = addBallIndex == touchedBallIndex ? addBallIndex + 1 : addBallIndex - 1;
            Vector3 TBallPos = ballList[neighbourBall].transform.position;

            if (Vector3.Distance(ABallPos, ballList[addBallIndex + 1].transform.position) <= 3)
                return true;
        }

        return false;
    }

    // Move the section head a unit forward along the path when new ball is added
    private void PushSectionForwardByUnit()
    {
        int sectionKey = sectionData.GetSectionKey(addBallIndex);
        int sectionKeyVal = sectionData.ballSections[sectionKey];

        float modifiedDistance;
        Vector3 tangent;

        GetComponent<BGCcMath>().CalcPositionByClosestPoint(ballList[sectionKeyVal].transform.position, out modifiedDistance);
        modifiedDistance += ballRadius;

        if (addBallIndex >= HeadballIndex)
            distance = modifiedDistance;

        Vector3 movedPos = GetComponent<BGCcMath>().CalcPositionAndTangentByDistance(modifiedDistance, out tangent);
        ballList[sectionKeyVal].transform.DOMove(movedPos, 1);
        ballList[sectionKeyVal].transform.rotation = Quaternion.LookRotation(tangent);
    }

    // Join the sections which were divided when balls were removed
    // Just check the current head with the next value if they are close
    private void JoinStoppedSections(int currentIdx, int nextSectionIdx)
    {
        float nextSecdist;
        GetComponent<BGCcMath>().CalcPositionByClosestPoint(ballList[nextSectionIdx].transform.position, out nextSecdist);

        if (nextSecdist - distance <= ballRadius)
        {
            int nextSectionKeyVal;
            sectionData.ballSections.TryGetValue(nextSectionIdx, out nextSectionKeyVal);
            HeadballIndex = nextSectionKeyVal;

            MergeSections(currentIdx, nextSectionKeyVal);
            //RemoveMatchedBalls(nextSectionIdx, ballList[nextSectionIdx]);

            if (ballList.Count > 0)
            {
                GetComponent<BGCcMath>().CalcPositionByClosestPoint(ballList[HeadballIndex].transform.position, out nextSecdist);
                distance = nextSecdist;
            }
        }
    }

    private void MergeSections(int currentIdx, int nextSectionKeyVal)
    {
        sectionData.ballSections.Remove(currentIdx - 1);
        sectionData.ballSections[int.MaxValue] = nextSectionKeyVal;
    }

    // Check if the added new ball has reached its correct position on the path along the curve
    // Also remove the match ball upon reaching the position
    // set a flag to know if the ball has reached its correct position
    private void AddNewBallAndDeleteMatched()
    {
        // check if the ball position is on the path
        int sectionKey = sectionData.GetSectionKey(addBallIndex);
        int sectionKeyVal = sectionData.ballSections[sectionKey];

        float neighbourDist = 0;
        Vector3 currentTangent;
        Vector3 actualPos = Vector3.zero;
        Vector3 neighbourPos = Vector3.zero;

        int end = sectionKey == int.MaxValue ? ballList.Count - 1 : sectionKey;
        if (addBallIndex == end)
        {
            neighbourPos = ballList[addBallIndex - 1].transform.position;
            GetComponent<BGCcMath>().CalcPositionByClosestPoint(neighbourPos, out neighbourDist);
            actualPos = GetComponent<BGCcMath>().CalcPositionAndTangentByDistance(neighbourDist - ballRadius, out currentTangent);
        }
        else
        {
            neighbourPos = ballList[addBallIndex + 1].transform.position;
            GetComponent<BGCcMath>().CalcPositionByClosestPoint(neighbourPos, out neighbourDist);
            actualPos = GetComponent<BGCcMath>().CalcPositionAndTangentByDistance(neighbourDist + ballRadius, out currentTangent);
        }

        Vector2 currentPos = new Vector2(ballList[addBallIndex].transform.position.x, ballList[addBallIndex].transform.position.y);
        float isNear = Vector2.Distance(actualPos, currentPos);

        if (isNear <= 0.1f)
        {
            RemoveMatchedBalls(addBallIndex, ballList[addBallIndex]);
            addBallIndex = -1;
        }
    }

    // Called by the collided new ball on collision with the active balls on the path
    private void RemoveMatchedBalls(int index, GameObject go)
    {
        int front = index;
        int back = index;
        int ballType = go.GetComponent<Ball>().type;

        //Color ballColor = go.GetComponent<Renderer>().material.GetColor("_Color");

        int sectionKey = sectionData.GetSectionKey(index);
        int sectionKeyVal;
        sectionData.ballSections.TryGetValue(sectionKey, out sectionKeyVal);

        // Check if any same color balls towards the front side
        for (int i = index - 1; i >= sectionKeyVal; i--)
        {
            int currentBallType = ballList[i].GetComponent<Ball>().type;

            //Color currrentBallColor = ballList[i].GetComponent<Renderer>().material.GetColor("_Color");
            //if (ballColor == currrentBallColor)
            if (ballType == currentBallType)
                    front = i;
            else
                break;
        }

        // Check if any same color balls towards the back side
        int end = sectionKey == int.MaxValue ? ballList.Count - 1 : sectionKey;
        for (int i = index + 1; i <= end; i++)
        {
            int currentBallType = ballList[i].GetComponent<Ball>().type;
            //Color currrentBallColor = ballList[i].GetComponent<Renderer>().material.GetColor("_Color");
            //if (ballColor == currrentBallColor)
            if (ballType == currentBallType)
                back = i;
            else
                break;
        }

        // If atleast 3 balls in a row are found at the new balls position
        if (back - front >= 0)
        {
            // Modify the headballIndex only if the remove section is in the moving section
            if (back > HeadballIndex)
            {
                // whole back section will be removed change the headIndex to the front section value
                if (front == sectionKeyVal && back == ballList.Count - 1)
                {
                    if (sectionData.ballSections.Count > 1)
                    {
                        int nextSectionFront;
                        sectionData.ballSections.TryGetValue(front - 1, out nextSectionFront);
                        HeadballIndex = nextSectionFront;
                    }
                }
                // if the remove section is less that the back i.e front and middle part of the moving section
                else
                {
                    if (front >= sectionKeyVal && back != ballList.Count - 1)
                    {
                        HeadballIndex = front;
                    }
                }
            }
            else
            {
                HeadballIndex -= (back - front + 1);
            }

            Debug.Log("HEAD INDEX:" + HeadballIndex);

            RemoveBalls(front, back - front + 1);

            if (back > HeadballIndex && ballList.Count > 0 && HeadballIndex != -1)
                GetComponent<BGCcMath>().CalcPositionByClosestPoint(ballList[HeadballIndex].transform.position, out distance);
        }
    }

    // Remove balls from the list also from the ballsContainer in scene
    private void RemoveBalls(int atIndex, int range)
    {
        for (int i = 0; i < range; i++)
        {
            ballList[atIndex + i].transform.parent = removedBallsContainer.transform;
            ballList[atIndex + i].SetActive(false);
        }
        ballList.RemoveRange(atIndex, range);

        OnDeleteModifySections(atIndex, range);
    }

    private void OnDeleteModifySections(int atIndex, int range)
    {
        int sectionKey = sectionData.GetSectionKey(atIndex);

        int sectionKeyVal;
        sectionData.ballSections.TryGetValue(sectionKey, out sectionKeyVal);

        // completely remove the section
        if (atIndex == sectionKeyVal && atIndex + range == ballList.Count + range)
        {
            sectionData.DeleteEntireSection(atIndex, range, sectionKey, ballList.Count);
        }
        else
        {
            sectionData.DeletePartialSection(atIndex, range, sectionKey, sectionKeyVal, ballList.Count);
        }
    }

    // Check if the active section front ball matches with the back of the next section
    private bool CheckIfActiveEndsMatch()
    {
        if (sectionData.ballSections.Count <= 1)
            return false;
        int headBallType = ballList[HeadballIndex].GetComponent<Ball>().type;
        int nextSectionEndType = ballList[HeadballIndex - 1].GetComponent<Ball>().type;
        //Color headBallColor = ballList[headballIndex].GetComponent<Renderer>().material.GetColor("_Color");
        //Color nextSectionEndColor = ballList[headballIndex - 1].GetComponent<Renderer>().material.GetColor("_Color");

        // if (headBallColor == nextSectionEndColor)
        if (headBallType == nextSectionEndType)
            return true;

        return false;
    }

    // Move the section to the front of the active section
    private void MergeActiveEnds()
    {
        int sectionKey = HeadballIndex - 1;
        int sectionKeyVal = sectionData.ballSections[sectionKey];

        int movingBallCount = 1;
        float sectionHeadDist;
        GetComponent<BGCcMath>().CalcPositionByClosestPoint(ballList[sectionKey].transform.position, out sectionHeadDist);
        sectionHeadDist -= mergeSpeed * Time.deltaTime;

        Vector3 tangent;
        Vector3 trailPos = GetComponent<BGCcMath>().CalcPositionAndTangentByDistance(sectionHeadDist, out tangent);
        ballList[sectionKey].transform.DOMove(trailPos, 0.3f)
            .SetEase(mergeEaseType);

        for (int i = sectionKey - 1; i >= sectionKeyVal; i--)
        {
            float currentBallDist = sectionHeadDist + movingBallCount * ballRadius;
            trailPos = GetComponent<BGCcMath>().CalcPositionAndTangentByDistance(currentBallDist, out tangent);

            ballList[i].transform.DOMove(trailPos, 0.3f)
                .SetEase(easeType);
            ballList[i].transform.rotation = Quaternion.LookRotation(tangent);

            movingBallCount++;
        }
    }

    // Merge the Stopped End balls
    private void MergeIfStoppedEndsMatch()
    {

    }
}
