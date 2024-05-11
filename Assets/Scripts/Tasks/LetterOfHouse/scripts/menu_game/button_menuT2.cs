namespace Task2
{
    using System.Collections.Generic;
    using System.Linq;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    public class button_menuT2 : MonoBehaviour
    {
        //объект канваса для удаления его после нажатия кнопки выход
        public GameObject canvas;
        //флаг который переходит к скрипту удаления меню после нажатия кнопок
        public bool cloze_flag;
        //Координаты родительского объекта для создания объекта поверх канваса
        public Transform parent;

        public GameObject housePrefab;
        public GameObject letterPrefab;

        public string[] Letters = new string[5];
        public string[] HouseEASY = new string[4];
        public string[] HouseMEDIUM = new string[6];
        public string[] HouseHARD = new string[10];


        //Массив точек для букв что бы они создавались в левой части экрана
        public List<Transform> spawnPointsLetters;
        //Массив точек для домов что бы они создавались в правой части экрана
        public List<Transform> spawnPointsHouses;
        //переменные для победы
        public int winnigNumber = 1;


        private void initHouse(string letter, int spawn)
        {
            GameObject newHouse = Instantiate(letterPrefab, spawnPointsHouses[spawn].transform.position, Quaternion.identity, parent);
            HouseT2 newHouseClass = newHouse.GetComponent<HouseT2>();
            newHouseClass.setText(letter);
        }
        private void GenaretionLetter()
        {
            spawnPointsLetters = new List<Transform>(spawnPointsLetters);
            int[] pointLetters = new int[Letters.Length];
            int i = 0;
            while (i < Letters.Length)
            {
                int spawnLetter = Random.Range(0, spawnPointsLetters.Count);
                if (pointLetters.Contains(spawnLetter))
                {
                    continue;
                }
                else
                {
                    GameObject newLetter = Instantiate(housePrefab, spawnPointsLetters[spawnLetter].transform.position, Quaternion.identity, parent);
                    HouseT2 newLetterClass = newLetter.GetComponent<HouseT2>();
                    newLetterClass.setText(Letters[i]);
                    pointLetters[i] = spawnLetter;
                    i++;
                }
            }
        }

        //Метод генерации домов и букв
        private void Generation(string[] house)
        {
            if (house.Length == HouseHARD.Length) winnigNumber = house.Length - 2;
            else winnigNumber = house.Length;
            cloze_flag = true;
            GenaretionLetter();
            spawnPointsHouses = new List<Transform>(spawnPointsHouses);
            int i = 0;

            int[] pointNUmbers = new int[house.Length];


            while (i < house.Length)
            {
                int spawnHouse = Random.Range(0, spawnPointsHouses.Count);
                if (pointNUmbers.Contains(spawnHouse))
                {
                    continue;
                }
                else
                {
                    initHouse(house[i], spawnHouse);
                    pointNUmbers[i] = spawnHouse;
                    i++;
                }
            }
        }


        public void ButClickEasy()
        {
            Generation(HouseEASY);
        }

        public void ButClickNormal()
        {
            Generation(HouseMEDIUM);
        }
        public void ButClickHard()
        {
            Generation(HouseHARD);
        }

        public void ButClickBack()
        {
            Game.closeAllUI();
            SceneManager.LoadSceneAsync(0, LoadSceneMode.Single);
        }
    }
}