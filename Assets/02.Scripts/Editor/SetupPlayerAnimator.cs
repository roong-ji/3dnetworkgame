using UnityEditor;
using UnityEngine;
using UnityEditor.Animations;

public class SetupPlayerAnimator : EditorWindow
{
    [MenuItem("Tools/Setup Player Animator")]
    public static void CreateAnimatorController()
    {
        // 1. 애니메이터 컨트롤러 생성 경로
        string assetPath = "Assets/02.Scripts/Player/PlayerAnimatorController.controller";
        
        // 만약 이미 있다면 삭제하고 다시 만듦
        if (AssetDatabase.LoadAssetAtPath<AnimatorController>(assetPath) != null)
        {
            AssetDatabase.DeleteAsset(assetPath);
        }

        // 2. 컨트롤러 생성
        AnimatorController controller = AnimatorController.CreateAnimatorControllerAtPath(assetPath);
        Debug.Log($"Created AnimatorController at {assetPath}");

        // 3. 파라미터(Parameter) 추가
        // PlayerMoveAbility: _animator.SetFloat("Move", direction.magnitude);
        controller.AddParameter("Move", AnimatorControllerParameterType.Float);
        
        // PlayerAttackAbility: _animator.SetTrigger($"Attack{animationNumber}");
        controller.AddParameter("Attack1", AnimatorControllerParameterType.Trigger);
        controller.AddParameter("Attack2", AnimatorControllerParameterType.Trigger);
        controller.AddParameter("Attack3", AnimatorControllerParameterType.Trigger);

        // 4. Base Layer 가져오기
        AnimatorStateMachine rootStateMachine = controller.layers[0].stateMachine;

        // 5. 상태(State) 생성
        // 이동 (Idle/Run 혼합용 Blend Tree)
        BlendTree blendTree;
        AnimatorState movementState = controller.CreateBlendTreeInController("Movement", out blendTree);
        blendTree.blendType = BlendTreeType.Simple1D;
        blendTree.blendParameter = "Move";
        // 실제 애니메이션 클립(Motion)은 에디터에서 직접 할당해주셔야 합니다.
        blendTree.AddChild(null, 0f); // Idle (Move = 0)
        blendTree.AddChild(null, 1f); // Walk (Move = 1)
        blendTree.AddChild(null, 2f); // Run (Move = 2 - 대시 중인 경우 스피드 비율 등에 따라 임의 설정)
        
        rootStateMachine.defaultState = movementState; // 기본 상태로 지정

        // 공격 상태들 생성
        AnimatorState attack1State = rootStateMachine.AddState("Attack1");
        AnimatorState attack2State = rootStateMachine.AddState("Attack2");
        AnimatorState attack3State = rootStateMachine.AddState("Attack3");

        // 6. 트랜지션(Transition) 설정: AnyState -> Attack
        AnimatorStateTransition trans1 = rootStateMachine.AddAnyStateTransition(attack1State);
        trans1.AddCondition(AnimatorConditionMode.If, 0, "Attack1");
        trans1.hasExitTime = false;
        trans1.duration = 0.1f;

        AnimatorStateTransition trans2 = rootStateMachine.AddAnyStateTransition(attack2State);
        trans2.AddCondition(AnimatorConditionMode.If, 0, "Attack2");
        trans2.hasExitTime = false;
        trans2.duration = 0.1f;

        AnimatorStateTransition trans3 = rootStateMachine.AddAnyStateTransition(attack3State);
        trans3.AddCondition(AnimatorConditionMode.If, 0, "Attack3");
        trans3.hasExitTime = false;
        trans3.duration = 0.1f;

        // 7. 공격 끝난 후 기본 상태로 돌아오기
        AnimatorStateTransition returnTrans1 = attack1State.AddTransition(movementState);
        returnTrans1.hasExitTime = true;
        returnTrans1.exitTime = 1.0f; // 애니메이션 끝까지 재생
        returnTrans1.duration = 0.25f;

        AnimatorStateTransition returnTrans2 = attack2State.AddTransition(movementState);
        returnTrans2.hasExitTime = true;
        returnTrans2.exitTime = 1.0f;
        returnTrans2.duration = 0.25f;

        AnimatorStateTransition returnTrans3 = attack3State.AddTransition(movementState);
        returnTrans3.hasExitTime = true;
        returnTrans3.exitTime = 1.0f;
        returnTrans3.duration = 0.25f;

        // 8. 변경사항 저장
        AssetDatabase.SaveAssets();
        
        EditorUtility.DisplayDialog("완료", "PlayerAnimatorController 생성이 완료되었습니다.\n경로: " + assetPath, "확인");
    }
}
