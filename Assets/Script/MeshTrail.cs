using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshTrail : MonoBehaviour
{
    public float activeTime=2f;
    public bool isTrailActive;
    private SkinnedMeshRenderer[] skinnedMeshRenderers ;

    [Header ("Mesh Realated")]
    public float meshRefreshRate = 0.1f;
    public float meshDestroyDelay=1f;
    public Transform positionToSpawn;

    [Header ("Material")]
    public Material mat;
    [SerializeField]
    public string shaderVarRef;
    public float shaderVarRate = 0.1f;
    public float shaderVarRefreshRate =0.5f;

    // Update is called once per frame
    void Update()
    {
         if (Input.GetKeyDown(KeyCode.Mouse1) && !isTrailActive){
            isTrailActive = true;
            StartCoroutine(ActivateTrail(activeTime));
         }
    }

    IEnumerator ActivateTrail (float timeActive)
    {
        while (timeActive > 0){

            timeActive -= meshRefreshRate;

            if(skinnedMeshRenderers == null)
                skinnedMeshRenderers = GetComponentsInChildren<SkinnedMeshRenderer>();

                for(int i=0; i<skinnedMeshRenderers.Length; i++)
                {
                    Debug.Log("Creating objectMesh"+ skinnedMeshRenderers.Length);

                    GameObject gObject= new GameObject();
                    gObject.transform.SetPositionAndRotation(positionToSpawn.position,positionToSpawn.rotation );
                    
                    MeshRenderer mr = gObject.AddComponent<MeshRenderer>();
                    MeshFilter mf =gObject.AddComponent<MeshFilter>();

                    Mesh mesh = new Mesh();
                    skinnedMeshRenderers[i].BakeMesh(mesh);

                    mf.mesh = mesh;
                    mr.material = mat;
                    StartCoroutine(AnimateMaterialFloat(mr.material,0,shaderVarRate,shaderVarRefreshRate));

                    Destroy (gObject,meshDestroyDelay);
                }
            

            yield return new WaitForSeconds(meshRefreshRate);
        }
        isTrailActive = false;
    }
    IEnumerator AnimateMaterialFloat(Material mat, float goal, float rate, float meshRefreshRate)
    {
        float valueToAnimate = mat.GetFloat(shaderVarRef);

        while (valueToAnimate>goal)
        {
            valueToAnimate -=rate;
            mat.SetFloat(shaderVarRef,valueToAnimate);
            yield return new WaitForSeconds(meshRefreshRate);
        }
    }
}
