using Configuration;
using RussianMunchkin.Common.Time;
using UnityEngine;
using View;

namespace Core
{
    public class EntryPoint: MonoBehaviour
    {
        [SerializeField] private  UIManager _uiManager;
        [SerializeField] private ConfigurationSo _configurationSo;
        
    
        private TimersManager _timersManager;
        private UnityTimeController _timeController;
        private ClientController _clientController;

        private void Awake()
        {
            _timeController = new UnityTimeController();
            _timersManager = new TimersManager(_timeController);
            _clientController = new ClientController(_uiManager, _configurationSo);
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
    }
}