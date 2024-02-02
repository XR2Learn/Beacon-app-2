using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicMath {

    public static bool approximate_equality_of (float _actual, float _abstract, 
        float _precision) {

        if (Mathf.Abs (_abstract - _actual) <= _precision)
            return true;
        else
            return false;

    }


    public static bool approximate_equality_of (List<float> _vector1, List<float> _vector2,
    float _precision) {

        if (_vector1.Count == _vector2.Count) {
            for (int i = 0; i < _vector1.Count; i++)
                if (!approximate_equality_of (_vector1[i], _vector2[i], _precision))
                    return false;
            return true;
        } else
            return false;

    }




    public static bool approximate_proximity_of (Vector3 P0, Vector3 P1, float _precision) {

        float d = Mathf.Sqrt (Mathf.Pow ((P0.x - P1.x), 2F) + Mathf.Pow ((P0.z - P1.z), 2F));

        if (d <= _precision)
            return true;
        else
            return false;

    }



    public static float square (float _number) {
        return _number * _number;
    }



    public static float similarity (List<float> _vector1, List<float> _vector2) {

        if (_vector1.Count == _vector2.Count) {

            float dot_product = 0F;
            float vector1_magnitude = 0F;
            float vector2_magnitude = 0F;

            for (int i = 0; i < _vector1.Count; i++) {
                dot_product += _vector1[i] * _vector2[i];
                vector1_magnitude += _vector1[i] * _vector1[i];
                vector2_magnitude += _vector2[i] * _vector2[i];
            }

            vector1_magnitude = Mathf.Sqrt (vector1_magnitude);
            vector2_magnitude = Mathf.Sqrt (vector2_magnitude);

            return dot_product / (vector1_magnitude * vector2_magnitude);

        } else
            return -1F;

    }


}
