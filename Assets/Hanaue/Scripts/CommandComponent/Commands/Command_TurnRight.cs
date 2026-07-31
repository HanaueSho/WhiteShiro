/*
    Command_TurnRight.cs
    20260728  hanaue sho
    コマンド右旋回
*/
using System;
using System.Collections;
using UnityEngine;


[CreateAssetMenu(fileName = "Command_TurnRight", menuName = "Scriptable Objects/Command/TurnRight")]
public class Command_TurnRight : CommandComponent
{
    public override IEnumerator Command(CommandPlayer owner, Action<bool> result)
    {
        base.Command(owner, result);

        // ----- 回転処理 -----
        Quaternion startRotation = owner.transform.rotation;
        Quaternion targetRotation = owner.transform.rotation * Quaternion.Euler(new Vector3(0.0f, 90.0f, 0.0f));
        float elapsedTime = 0.0f;
        float moveDuration = 0.5f;
        while (elapsedTime < moveDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = Mathf.Clamp01(elapsedTime / moveDuration);
            owner.transform.rotation = Quaternion.Slerp(startRotation, targetRotation, t);

            yield return null; // 次フレームまで待つ
        }
        owner.transform.rotation = targetRotation;

        result(true);
        yield break;
    }
}
