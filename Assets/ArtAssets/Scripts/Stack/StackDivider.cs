using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackDivider : MonoBehaviour
{
    //ı
    [SerializeField] Transform stackPrefab;
     
    Transform lastStandingStack;
    Transform previousStandingStack;
    
    public void GenerateStackHistory(Transform _previousStandingStack, Transform _lastStandingStack )
    { 
        lastStandingStack = _lastStandingStack;
        previousStandingStack = _previousStandingStack;
    }
    public void DivideObject(float wastedArea)
    {
        SetStandingPieceScale(wastedArea);
        float fallingPieceAlignPosition = GetFallingPieceAlignPosition();
        SetStandingPieceAlignPosition(fallingPieceAlignPosition);
        GenerateFallingPiece(positionX: fallingPieceAlignPosition, scaleX: wastedArea);

    }

   
    private float GetFallingPieceAlignPosition()
    {
        float fallingPieceAlignPosition = 0;
        bool alignPositionIsRight = previousStandingStack.transform.position.x > lastStandingStack.transform.position.x;

        if(alignPositionIsRight)
        {
            fallingPieceAlignPosition = previousStandingStack.transform.position.x - (previousStandingStack.transform.localScale.x / 2);
        } else
        {
            fallingPieceAlignPosition = previousStandingStack.transform.position.x + (previousStandingStack.transform.localScale.x / 2);

        }
        return fallingPieceAlignPosition;
    }


    private void SetStandingPieceScale(float wastedArea)
    {
        float standingStackScaleX = lastStandingStack.transform.localScale.x - wastedArea;
        lastStandingStack.transform.localScale = new Vector3(standingStackScaleX, lastStandingStack.transform.localScale.y, lastStandingStack.transform.localScale.z);
    }
 private void GenerateFallingPiece(float positionX, float scaleX)
    {
        if(scaleX == 0)
        {
            return;
        }
        GameObject fallingPiece = Instantiate(stackPrefab.gameObject, new Vector3(positionX, lastStandingStack.transform.position.y, lastStandingStack.transform.position.z), Quaternion.identity);
        fallingPiece.AddComponent<Rigidbody>();
        fallingPiece.transform.localScale = new Vector3(scaleX, lastStandingStack.localScale.y, lastStandingStack.localScale.z);
        fallingPiece.GetComponent<MeshRenderer>().material = lastStandingStack.GetComponent<MeshRenderer>().material;
        Destroy(fallingPiece, 5);
        

    }  
    private void SetStandingPieceAlignPosition(float fallingPieceAlignPosition)
    {
        float standingPieceAlignPosition;
        if(fallingPieceAlignPosition < 0)
        {
            standingPieceAlignPosition = fallingPieceAlignPosition + (lastStandingStack.localScale.x / 2);
        } else
        {
            standingPieceAlignPosition = fallingPieceAlignPosition - (lastStandingStack.localScale.x / 2);

        }
        lastStandingStack.transform.position = new Vector3(standingPieceAlignPosition, lastStandingStack.transform.position.y, lastStandingStack.transform.position.z);
    }
   
}
