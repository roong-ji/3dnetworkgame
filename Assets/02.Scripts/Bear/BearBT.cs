using System.Collections.Generic;
using UnityEngine;

public class BearBT : BT
{
    [Header("Movement Settings")]
    public float moveSpeed = 3f;
    public float chaseSpeed = 5f;
    public float rotationSpeed = 10f;

    [Header("Detection Settings")]
    public float detectRange = 10f;
    public float attackRange = 1.5f;

    private CharacterController _controller;
    private Animator _animator;
    private Transform _target;

    protected override Node SetupTree()
    {
        _controller = GetComponent<CharacterController>();
        _animator = GetComponent<Animator>();

        // 플레이어 스캔
        Node scanTarget = new ActionNode(() => {
            _target = FindNearestPlayer();
            return _target != null ? State.Success : State.Failure;
        });

        // 공격 조건 및 액션
        Node isInAttackRange = new ConditionNode(() => _target != null && Vector3.Distance(transform.position, _target.position) < attackRange);
        Node attackAction = new ActionNode(PerformAttack);

        // 추격 조건 및 액션
        Node isInDetectRange = new ConditionNode(() => _target != null && Vector3.Distance(transform.position, _target.position) < detectRange);
        Node chaseAction = new ActionNode(PerformChase);

        // 순찰 액션
        Node wanderAction = new ActionNode(PerformWander);

        // --- 2. 중간 단계 시퀀스/셀렉터 조립 및 이름 부여 ---

        // 공격 시퀀스 (거리 체크 -> 공격 실행)
        Node attackSequence = new SequenceNode(new List<Node> { isInAttackRange, attackAction });

        // 추격 시퀀스 (탐지 체크 -> 추격 실행)
        Node chaseSequence = new SequenceNode(new List<Node> { isInDetectRange, chaseAction });

        // 전투 셀렉터 (공격 우선, 안되면 추격)
        Node combatSelector = new SelectorNode(new List<Node> { attackSequence, chaseSequence });

        // 인지 시퀀스 (스캔 -> 전투 결정)
        Node detectionSequence = new SequenceNode(new List<Node> { scanTarget, combatSelector });

        // 루트 셀렉터 (인지/전투 시퀀스 -> 실패 시 순찰)
        Node rootSelector = new SelectorNode(new List<Node> { detectionSequence, wanderAction });

#if UNITY_EDITOR
        // --- 3. 에디터 전용 이름 할당 (이게 핵심) ---
        // 최하단 노드
        scanTarget.Name = "플레이어 스캔";
        isInAttackRange.Name = "공격 사거리 체크";
        attackAction.Name = "공격 실행";
        isInDetectRange.Name = "탐지 범위 체크";
        chaseAction.Name = "추격 실행";
        wanderAction.Name = "순찰/대기";

        // 중간/상위 노드
        attackSequence.Name = "공격 시퀀스";
        chaseSequence.Name = "추격 시퀀스";
        combatSelector.Name = "전투 행동 결정";
        detectionSequence.Name = "인지 및 추적";
        rootSelector.Name = "곰 메인 로직";
#endif
        return rootSelector;
    }
    
    // --- Action Methods ---

    private State PerformAttack()
    {
        _animator.SetTrigger("Attack1");
        return State.Success;
    }

    private Transform FindNearestPlayer()
    {
        // 씬에 있는 모든 Player 태그 오브젝트를 다 가져옴
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length == 0) return null;

        Transform nearest = null;
        float minDistance = float.MaxValue;

        foreach (var p in players)
        {
            float dist = Vector3.Distance(transform.position, p.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                nearest = p.transform;
            }
        }

        // 감지 거리 밖의 플레이어는 무시하고 싶다면 여기서 필터링
        return (minDistance <= detectRange) ? nearest : null;
    }

    private State PerformChase()
    {
        if (_target == null) return State.Failure;
        MoveTowards(_target.position, chaseSpeed);
        _animator.SetBool("Run Forward", true);
        return State.Running;
    }

    private State PerformWander()
    {
        // 간단한 산책 로직 (여기선 예시로 가만히 있게 처리하거나 랜덤 방향 이동)
        _animator.Rebind();
        return State.Running;
    }

    private void MoveTowards(Vector3 target, float speed)
    {
        // 방향 계산
        Vector3 direction = (target - transform.position).normalized;
        direction.y = 0; // 높이 고정

        // 회전
        if (direction != Vector3.zero)
        {
            Quaternion targetRot = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        }

        // CharacterController 이동 (중력 포함)
        Vector3 velocity = direction * speed;
        velocity.y = Physics.gravity.y; 
        _controller.Move(velocity * Time.deltaTime);
    }
}
