using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BezierCurveLine
{
    public List<Vector3> m_Point_Influence = new List<Vector3>();
    private int m_nInFormular { get => m_Point_Influence.Count - 1; }
        

    public BezierCurveLine()
    {
        while (m_Point_Influence.Count < 2)
        {
            m_Point_Influence.Add(Vector3.zero);
        }
    }

    private float Combination(int n, int i)
    {
        return Factorial(n) / (Factorial(i) * Factorial(n - i));
    }

    private int Factorial(int i)
    {
        if (i <= 1)
            return 1;
        return i * Factorial(i - 1);
    }

    /// <summary>
    /// Use This method to Update the Bezier Line.
    /// </summary>
    /// <param name="SpeedUpdate">Speed on Update the Bezier Curve. 
    /// Time.deltaTime is default.</param>
    /// <returns>Return a Current Point on Bezier Curve.</returns>
    public Vector3 GetBezierResultInLine(float time)
    {

        time = Mathf.Clamp01(time);

        Vector3 returnPos = Vector3.zero;

        for (int j = 0; j <= 2; j++)
        {
            for (int i = 0; i <= m_Point_Influence.Count - 1; i++)
            {
                switch (j)
                {
                    case 0:
                        returnPos.x += Combination(m_nInFormular, i) *
                    Mathf.Pow(1 - time, m_nInFormular - i) *
                    Mathf.Pow(time, i) * m_Point_Influence[i].x;
                        break;
                    case 1:
                        returnPos.y += Combination(m_nInFormular, i) *
                    Mathf.Pow(1 - time, m_nInFormular - i) *
                    Mathf.Pow(time, i) * m_Point_Influence[i].y;
                        break;
                    case 2:
                        returnPos.z += Combination(m_nInFormular, i) *
                    Mathf.Pow(1 - time, m_nInFormular - i) *
                    Mathf.Pow(time, i) * m_Point_Influence[i].z;
                        break;
                }
            }

        }

        return returnPos;
    }

    public void SetBezierPoint(params Vector3[] points)
    {
        m_Point_Influence = points.ToList();
    }

    public void SetBezierPoint(params Transform[] points) 
    {

        m_Point_Influence.Clear();

        foreach(Transform t in points)
        {
            m_Point_Influence.Add(t.position);
        }
    }

}
