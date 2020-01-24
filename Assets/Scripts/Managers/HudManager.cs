namespace STUDENT_NAME
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.UI;
    using SDD.Events;
    using System;

    public class HudManager : Manager<HudManager>
	{

		//[Header("HudManager")]
		#region Labels & Values
		// TO DO
		#endregion

		#region Manager implementation
		protected override IEnumerator InitCoroutine()
		{
			yield break;
		}
        #endregion

        public override void SubscribeEvents()
        {
            base.SubscribeEvents();

            EventManager.Instance.AddListener<FuelAmountChangedEvent>(FuelAmountChanged);

        }

       

        public override void UnsubscribeEvents()
        {
            base.UnsubscribeEvents();

            EventManager.Instance.RemoveListener<FuelAmountChangedEvent>(FuelAmountChanged);
        }

        private void FuelAmountChanged(FuelAmountChangedEvent e)
        {

        }

        #region Callbacks to GameManager events
        protected override void GameStatisticsChanged(GameStatisticsChangedEvent e)
		{
			//TO DO
		}
		#endregion

	}
}