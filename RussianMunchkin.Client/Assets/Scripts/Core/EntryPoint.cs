using System;
using RussianMunchkin.Common.Time;
using UnityEngine;
using View;

namespace Core
{
    public class EntryPoint: MonoBehaviour
    {
        [SerializeField] private  UIManager _uiManager;
    
        private TimersManager _timersManager;
        private UnityTimeController _timeController;
        private ClientController _clientController;

        private void Awake()
        {
            _timeController = new UnityTimeController();
            _timersManager = new TimersManager(_timeController);
            _clientController = new ClientController(
                _uiManager.ConnectingView,_uiManager.ServerHandlerView, _uiManager.AuthUsernameView, _uiManager.RoomsView, _uiManager.GameView);
        }

        private void OnEnable()
        {
            _clientController.Enable();
        }
        private void Update()
        {
            _timeController.Update();
        }

        private void FixedUpdate()
        {
            _clientController.Update();
        }

        private void OnDisable()
        {
            _clientController.Disable();

        }

        private void OnDestroy()
        {
            //throw new NotImplementedException();
        }
    }
}