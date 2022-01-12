using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{

    private static List<Vector3> l_barycentricMeshNormals = new List<Vector3>();
    private static List<int> l_barycentricMeshIdx = new List<int>();

    public static Vector3 InterpolateFixedVector(Vector3 lastPosition, Vector3 targetPosition)
    {
        return Vector3.Lerp(lastPosition, targetPosition, TimestepInterpolator.FIXED_INTERPOLATION_VALUE);
    }

    public static Quaternion InterpolateFixedQuaternion(Quaternion lastRotation, Quaternion targetRotation)
    {
        return Quaternion.Lerp(lastRotation, targetRotation, TimestepInterpolator.FIXED_INTERPOLATION_VALUE);
    }

    public static float GetFloatRatio(float target, float minimum, float maximum)
    {
        return (target - minimum) / (maximum - minimum);
    }

    public static float InterpolateFloat(float ratio, float minimum, float maximum)
    {
        return (minimum * (1.0f - ratio)) + (maximum * ratio);
    }

    public static float InterpolateFixedFloat(float lastFloat, float targetFloat)
    {
        return (lastFloat * (1.0f - TimestepInterpolator.FIXED_INTERPOLATION_VALUE)) + (targetFloat * TimestepInterpolator.FIXED_INTERPOLATION_VALUE);
    }

    public static float InterpolateFloatManual(float min, float max, float target)
    {
        return (min * (Constants.ONE - max)) +(target * max);
    }

    public static Color InterpolateFixedColor(Color lastColor, Color targetColor)
    {
        return Color.Lerp(lastColor, targetColor, TimestepInterpolator.FIXED_INTERPOLATION_VALUE);
    }

    public static Vector3 GetBaryCentricNormal(RaycastHit hitIn)
    {
        Vector3 BarycentricNormal = Vector3.zero;
        Vector3 BarycentricCoords = hitIn.barycentricCoordinate;

        MeshCollider meshCol = hitIn.collider as MeshCollider;
        if (meshCol == null || meshCol.sharedMesh == null)
        {
            return BarycentricNormal;
        }

        Mesh mesh = (hitIn.collider as MeshCollider).sharedMesh;
        mesh.GetNormals(l_barycentricMeshNormals);
        mesh.GetTriangles(l_barycentricMeshIdx, 0);

        Vector3 n0 = l_barycentricMeshNormals[l_barycentricMeshIdx[hitIn.triangleIndex * 3]]; //mesh.normals[mesh.triangles[hitIn.triangleIndex * 3 + 0]];
        Vector3 n1 = l_barycentricMeshNormals[l_barycentricMeshIdx[hitIn.triangleIndex * 3 + 1]];
        Vector3 n2 = l_barycentricMeshNormals[l_barycentricMeshIdx[hitIn.triangleIndex * 3 + 2]];

        BarycentricNormal = n0 * BarycentricCoords.x +
                            n1 * BarycentricCoords.y +
                            n2 * BarycentricCoords.z;

        BarycentricNormal = BarycentricNormal.normalized;

        // Transform local space normals to world space
        Transform hitTransform = hitIn.collider.transform;
        BarycentricNormal = hitTransform.TransformDirection(BarycentricNormal);

        return BarycentricNormal.normalized;
    }

}
