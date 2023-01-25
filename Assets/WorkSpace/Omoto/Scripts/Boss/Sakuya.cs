using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using CharacterState;
using EventState;
using Item;

namespace Boss
{
    public class Sakuya : BossProcessor
    {
        // 初期処理
        //============================================
        void Start()
        {
            // 共通初期処理
            Init();

            // ステート, ステータス初期化
            //============================================
            m_State = new IdleState();
            m_State.ExecAction = Idle;
        }

        // 更新処理
        //============================================
        void Update()
        {
            // ボス共通処理
            Exec();
        }

        protected override void CheckState()
        {

        }

        protected override void OnEvent(EventProcessor eventProcessor)
        {
            eventProcessor.Execute();
        }

        protected override void Idle()
        {
            
        }

        protected override void Move()
        {
            
        }

        protected override void Jump()
        {
            
        }

        protected override void Floating()
        {
            
        }

        protected override void Attack()
        {
            
        }
    }
}