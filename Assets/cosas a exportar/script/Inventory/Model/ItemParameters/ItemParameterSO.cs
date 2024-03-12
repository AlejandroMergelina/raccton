using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Inventory.Model
{
    [CreateAssetMenu]
    public class ItemParameterSO : ScriptableObject
    {

        [SerializeField]
        private string parameterName;
        public string ParameterName { get => parameterName; set => parameterName = value; }


    }
}