using EEA.MiniGames;
using EEA.Services;
using System.Collections.Generic;
using UnityEngine;

namespace EEA.Menu
{
    public class MenuMiniGameScroll : MonoBehaviour
    {
        [SerializeField] private Transform _content;
        [SerializeField] private MiniGameCard _miniGameCardPrefab;
        [SerializeField] private SnappyScroll _snappyScroll;

        private List<MiniGameCard> _miniGameCards = new List<MiniGameCard>();

        private void Start()
        {
            foreach (var miniGameData in MiniGameService.Instance.Settings.MiniGames)
            {
                var card = ServicesContainer.GlobalPool.Spawn(_miniGameCardPrefab);
                card.transform.SetParent(_content, false);
                card.Initialize(miniGameData);

                _miniGameCards.Add(card);
            }

            _snappyScroll.Setup();
        }

        private void OnDestroy()
        {
            foreach (var card in _miniGameCards)
            {
                ServicesContainer.GlobalPool.Despawn(card.gameObject);
            }
            _miniGameCards.Clear();
        }
    }
}