using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Positions : MonoBehaviour
{
    [SerializeField] private bool drawGizmos;
    [SerializeField] private Transform[] points;
    [SerializeField] private float startPositionZ;
    [SerializeField] private float startPositionX;
    [SerializeField] private float startPositionY;
    [SerializeField] private float spacingZ;
    [SerializeField] private float spacingX;
    [SerializeField] private float quantityPerColumns;

    private void Awake()
    {
        Calculate();        
    }


    public void Calculate()
    {
        float lines = points.Length / quantityPerColumns;

        float amount = points.Length;

        float axisZ = startPositionZ;
        float axisX = 0;

        int currentId = 0;


        for (int i = 0; i < lines; i++)
        {
            axisX = startPositionX;

            float poitsLine = Mathf.Clamp(amount, 0, quantityPerColumns);

            for (int y = 0; y < poitsLine; y++)
            {
                
                points[currentId].localPosition = new Vector3(axisX, startPositionY, axisZ);

                currentId++;
                amount--;

                axisX += spacingX;
            }

            axisZ += spacingZ;
        }
    }

    public Transform[] GetPoints()
    {
        return points;
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos)
        {
            float lines = points.Length / quantityPerColumns;

            float amount = points.Length;

            float axisZ = startPositionZ;
            float axisX = 0;

            int currentId = 0;


            for (int i = 0; i < lines; i++)
            {
                axisX = startPositionX;

                float poitsLine = Mathf.Clamp(amount, 0, quantityPerColumns);

                for (int y = 0; y < poitsLine; y++)
                {

                    points[currentId].localPosition = new Vector3(axisX, startPositionY, axisZ);

                    Gizmos.color = Color.red;

                    Gizmos.DrawSphere(points[currentId].position, 0.3f);

                    currentId++;
                    amount--;

                    axisX += spacingX;
                }

                axisZ += spacingZ;
            }
        }
        
    }

}




