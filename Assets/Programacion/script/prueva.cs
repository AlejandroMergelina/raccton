using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class prueva : MonoBehaviour
{
    [SerializeField]
    private MainCharacter1[] paco;

    void Start()
    {
        List<Character> objectList = new List<Character>();

        foreach(Character c in paco)
        {

            objectList.Add(c);

        }

        MergeSort(objectList, 0, objectList.Count - 1);

        foreach(Character c in objectList)
        {

            print(c.CharacterData.Speed.GetValue());

        }

    }

    public static void MergeSort(List<Character> list, int left, int right)
    {
        if (left < right)
        {
            int middle = (left + right) / 2;
            //MergeSort(list, left, middle);
            MergeSort(list, middle + 1, right);
            Merge(list, left, middle, right);
        }
    }

    public static void Merge(List<Character> list, int left, int middle, int right)
    {
        int leftSize = middle - left + 1;
        int rightSize = right - middle;

        List<Character> leftTemp = new List<Character>();
        List<Character> rightTemp = new List<Character>();

        for (int i = 0; i < leftSize; i++)
        {
            leftTemp.Add(list[left + i]);
        }
        for (int j = 0; j < rightSize; j++)
        {
            rightTemp.Add(list[middle + 1 + j]);
        }

        int leftIndex = 0;
        int rightIndex = 0;
        int mergedIndex = left;

        while (leftIndex < leftSize && rightIndex < rightSize)
        {
            if (leftTemp[leftIndex].CharacterData.Speed.GetValue() >= rightTemp[rightIndex].CharacterData.Speed.GetValue())
            {
                list[mergedIndex] = leftTemp[leftIndex];
                leftIndex++;
            }
            else
            {
                list[mergedIndex] = rightTemp[rightIndex];
                rightIndex++;
            }
            mergedIndex++;
        }

        while (leftIndex < leftSize)
        {
            list[mergedIndex] = leftTemp[leftIndex];
            leftIndex++;
            mergedIndex++;
        }

        while (rightIndex < rightSize)
        {
            list[mergedIndex] = rightTemp[rightIndex];
            rightIndex++;
            mergedIndex++;
        }
    }
}
