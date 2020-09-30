using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

namespace rpg
{
    public class MontgeronStory : GameStory
    {
        public AudioClip baseMusic;
        public AudioClip bossMusic;

        public GameObject explosionPrefab;

        public Sprite maximeSprite;
        public Sprite alteaSprite;
        public Sprite orionSprite;
        public Sprite luckySprite;

        public Teleporter entrance;
        public MovableEntity playerMovable;
        public MovableEntity maximeMovable;
        public MovableEntity alteaMovable;
        public MovableEntity orionMovable;
        public MovableEntity cerberusMovable;
        public MovableEntity luckyMovable;

        [Header("Positions")]
        public Vector3 startPos;
        public Vector3 morganePos1;
        public Vector3 morganePos2;
        public Vector3 catsPosition;
        public Vector3 maxPosition;

        public float speed_walk = 1.0f;
        public float speed_run = 5.0f;
        public List<string> firstDialogue;
        public string talk_altea = "Altéa : Euh, Morgane... Je crois qu'il dit la vérité.";
        public string talk_orion = "Orion : La présence maléfique que je ressentais... C'est lui.";
        public string talk_orion_dog1 = "Orion : ENSIL ENSCI ! Le Démon qui terrorise Limoges depuis la nuit des temps !";
        public string talk_orion_dog2 = "Orion : Mais que fait-il ici ? Morgane, protège-nous !";
        public string talk_dog = "Mais... Qu'est-ce que...";

        private bool wait = false;
        private int counter = 0;

        protected override IEnumerator Start()
        {
            yield return StartCoroutine(base.Start());

            player.attackEnabled = false;

            if (RpgManager.GetKey(SaveKey.defeatedCerberus) == 0)
            {
                yield return StartCoroutine(FindMaxime());
            }
            else if (RpgManager.GetKey(SaveKey.defeatedCerberus) == -1)
            {
                if (playerMovable == null)
                    playerMovable = player.gameObject.GetComponent<MovableEntity>();
                if (playerMovable == null)
                    playerMovable = player.gameObject.AddComponent<MovableEntity>();

                maximeMovable.transform.position = maxPosition;
                alteaMovable.transform.position = (catsPosition - Vector3.right);
                orionMovable.transform.position = (catsPosition + Vector3.right);
                luckyMovable.transform.position = morganePos1 + Vector3.down * 2;
                player.transform.position = morganePos1;
                
                player.overrideMovement = true;
                audioSource.clip = bossMusic;
                audioSource.Play();
                yield return new WaitForSeconds(1.0f);
                TransitionBattle trbattle = TransitionBattle.Instance;
                trbattle.onClosureFinished = () => { SceneManager.LoadScene("PokemonBattle", LoadSceneMode.Additive); };
                trbattle.StartSpiralCoroutine();
            }
            else if (RpgManager.GetKey(SaveKey.defeatedCerberus) == -2)
            {
                if (playerMovable == null)
                    playerMovable = player.gameObject.GetComponent<MovableEntity>();
                if (playerMovable == null)
                    playerMovable = player.gameObject.AddComponent<MovableEntity>();

                maximeMovable.transform.position = maxPosition;
                alteaMovable.transform.position = (catsPosition - Vector3.right);
                orionMovable.transform.position = (catsPosition + Vector3.right);
                luckyMovable.transform.position = morganePos1 + Vector3.down * 2;
                player.transform.position = morganePos1;

                player.overrideMovement = true;
                audioSource.Stop();

                yield return EndPokemonCoroutine();
            }
            else if (RpgManager.GetKey(SaveKey.defeatedCerberus) == 1)
            {
                cerberusMovable.gameObject.SetActive(false);
                alteaMovable.Sleep();
                alteaMovable.Hit();

                StartCoroutine(OrionRoamCoroutine());
            }
        }

        private IEnumerator FindMaxime()
        {
            entrance.gameObject.SetActive(false);
            player.overrideMovement = true;

            if (playerMovable == null)
                playerMovable = player.gameObject.GetComponent<MovableEntity>();
            if (playerMovable == null)
                playerMovable = player.gameObject.AddComponent<MovableEntity>();

            maximeMovable.transform.position = maxPosition;
            alteaMovable.transform.position = startPos;
            orionMovable.transform.position = startPos;
            cerberusMovable.transform.position = startPos;
            cerberusMovable.gameObject.SetActive(false);
            luckyMovable.transform.position = morganePos1 + Vector3.down * 2;
            luckyMovable.gameObject.SetActive(false);

            playerMovable.LookTowards(Vector3.up);
            alteaMovable.MoveTo(startPos - Vector3.right);
            orionMovable.MoveTo(startPos + Vector3.right);
            yield return new WaitWhile(() => alteaMovable.isMoving);

            alteaMovable.LookTowards(Vector3.up);
            orionMovable.LookTowards(Vector3.up);
            wait = true;
            player.Talk("Max ?", () => wait = false);
            yield return new WaitWhile(() => wait);

            player.EndTalk();
            playerMovable.MoveTo(morganePos1, speed_walk);
            yield return new WaitWhile(() => playerMovable.isMoving);

            wait = true;
            player.Talk("Maxime !", () => wait = false);
            yield return new WaitWhile(() => wait);

            player.EndTalk();
            playerMovable.MoveTo(morganePos2, speed_run);
            yield return new WaitUntil(() => playerMovable.isMoving == false);

            RpgManager.Instance.discussionInterface.SetImage(false, maximeSprite);
            counter = 0;
            while (counter < firstDialogue.Count)
            {
                wait = true;
                bool isMorgane = firstDialogue[counter].Split(':')[0] == "Morgane ";
                player.Dialog(isMorgane, firstDialogue[counter], () => wait = false);
                counter++;
                yield return new WaitWhile(() => wait);
            }

            player.EndTalk();
            alteaMovable.MoveTo(catsPosition - Vector3.right);
            orionMovable.MoveTo(catsPosition + Vector3.right);
            yield return new WaitWhile(() => alteaMovable.isMoving);

            orionMovable.LookTowards(Vector3.down);
            RpgManager.Instance.discussionInterface.SetImage(false, alteaSprite);
            wait = true;
            player.Dialog(false, talk_altea, () => wait = false);
            yield return new WaitWhile(() => wait);

            playerMovable.LookTowards(Vector3.down);
            RpgManager.Instance.discussionInterface.SetImage(false, orionSprite);
            wait = true;
            player.Dialog(false, talk_orion, () => wait = false);
            yield return new WaitWhile(() => wait);

            player.EndTalk();
            audioSource.clip = bossMusic;
            audioSource.Play();
            cerberusMovable.gameObject.SetActive(true);
            cerberusMovable.LookTowards(Vector3.up);
            alteaMovable.LookTowards(Vector3.down);
            playerMovable.MoveTo(morganePos1, speed_walk);
            yield return new WaitWhile(() => playerMovable.isMoving);

            wait = true;
            player.Dialog(false, talk_orion_dog1, () => wait = false);
            yield return new WaitWhile(() => wait);
            wait = true;
            player.Dialog(false, talk_orion_dog2, () => wait = false);
            yield return new WaitWhile(() => wait);
            player.EndTalk();
            wait = true;
            player.Talk(talk_dog, () => wait = false);
            yield return new WaitWhile(() => wait);

            player.EndTalk();
            cerberusMovable.MoveTo(morganePos1 + Vector3.down, speed_walk);
            yield return new WaitWhile(() => cerberusMovable.isMoving);

            cerberusMovable.Hit();
            
            RpgManager.SetKey(SaveKey.defeatedCerberus, -1);

            RpgManager.Instance.gameState = RpgManager.Instance.gameState ^ RpgManager.GameState.Minigame;
            TransitionBattle trbattle = TransitionBattle.Instance;
            trbattle.onClosureFinished = () => { SceneManager.LoadScene("PokemonBattle", LoadSceneMode.Additive); };
            trbattle.StartSpiralCoroutine();
        }

        private void OnDrawGizmos()
        {
            Gizmos.DrawSphere(startPos, 0.2f);
            Gizmos.DrawLine(startPos, morganePos1);
            Gizmos.DrawSphere(morganePos1, 0.2f);
            Gizmos.DrawLine(morganePos1, morganePos2);
            Gizmos.DrawSphere(morganePos2, 0.2f);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(catsPosition, 0.2f);
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(maxPosition, 0.2f);
        }

        public void OnEndPokemonBattle()
        {
            RpgManager.Instance.gameState = RpgManager.Instance.gameState ^ RpgManager.GameState.Minigame;

            RpgManager.SetKey(SaveKey.defeatedCerberus, -2);
            cerberusMovable.transform.position = luckyMovable.transform.position;
            StartCoroutine(EndPokemonCoroutine());
        }

        private IEnumerator EndPokemonCoroutine()
        {
            bool wait = true;
            
            RpgManager.Instance.discussionInterface.SetImage(false, alteaSprite);
            player.Dialog(false, "Altea : Tu l'as vaincu ! Tu as vaincu le démon ! Nous sommes sauvés !", () => wait = false);
            yield return new WaitWhile(() => wait);

            wait = true;
            RpgManager.Instance.discussionInterface.SetImage(false, orionSprite);
            player.Dialog(false, "Orion : Attendez, il se passe quelque chose ! Mais quelle est cette lumière ?", () => wait = false);
            yield return new WaitWhile(() => wait);

            player.EndTalk();

            wait = true;
            BossExplosion explosion = Instantiate(explosionPrefab).GetComponent<BossExplosion>();
            explosion.OnWhiteScreenEvent += () => wait = false;
            explosion.OnFinishedEvent += () => wait = false;

            cerberusMovable.Hit();

            yield return new WaitWhile(() => wait);

            wait = true;
            cerberusMovable.gameObject.SetActive(false);
            luckyMovable.gameObject.SetActive(true);
            luckyMovable.LookAt(morganePos1);
            
            yield return new WaitWhile(() => wait);

            orionMovable.MoveTo(morganePos1 + Vector3.up + Vector3.right);
            maximeMovable.MoveTo(morganePos1 + Vector3.up + Vector3.right);
            yield return new WaitWhile(() => maximeMovable.isMoving);
            maximeMovable.MoveTo(morganePos1 + Vector3.right);
            yield return new WaitWhile(() => maximeMovable.isMoving);

            maximeMovable.LookTowards(Vector3.down);

            audioSource.clip = baseMusic;
            audioSource.time = 12f;
            audioSource.Play();

            wait = true;
            RpgManager.Instance.discussionInterface.SetImage(false, maximeSprite);
            player.Dialog(false, "Maxime : Ca alors, il s'est transformé !", () => wait = false);
            yield return new WaitWhile(() => wait);

            wait = true;
            RpgManager.Instance.discussionInterface.SetImage(false, orionSprite);
            player.Dialog(false, "Orion : Et je ne sens plus aucune aura maléfique. Il semblerait que le démon ait disparu.", () => wait = false);
            alteaMovable.MoveTo(player.transform.position + Vector3.up + Vector3.left);
            yield return new WaitWhile(() => wait);

            wait = true;
            RpgManager.Instance.discussionInterface.SetImage(false, alteaSprite);
            player.Dialog(false, "Altea : Morgane a reussi à l'exorciser. Cette chienne était sous son emprise, c'est terrible.", () => wait = false);
            yield return new WaitWhile(() => wait);
            
            wait = true;
            player.Dialog(true, "Morgane : C'est pas faux.", () => wait = false);
            yield return new WaitWhile(() => wait);
            
            wait = true;
            RpgManager.Instance.discussionInterface.SetImage(false, luckySprite);
            player.Dialog(false, "????? : Merci de m'avoir libérée, Humaine. Tes amis ont raison. J'étais possédée par ce démon depuis bien longtemps.", () => wait = false);
            yield return new WaitWhile(() => wait);

            wait = true;
            player.Dialog(true, "Morgane : Si tu n'es plus le démon, qui es-tu ? Je m'appelle Morgane.", () => wait = false);
            yield return new WaitWhile(() => wait);

            wait = true;
            RpgManager.Instance.discussionInterface.SetImage(false, luckySprite);
            player.Dialog(false, "Lucky : Tu peux m'appeler Lucky. Cela symbolisera la chance que j'ai de vous avoir rencontrés, toi et Maxime.", () => wait = false);
            yield return new WaitWhile(() => wait);

            wait = true;
            RpgManager.Instance.discussionInterface.SetImage(false, luckySprite);
            player.Dialog(false, "Lucky : Il t'aime beaucoup tu sais ? Je crois qu'il avait quelque chose à te montrer.", () => wait = false);
            yield return new WaitWhile(() => wait);

            player.EndTalk();
            luckyMovable.MoveTo(luckyMovable.transform.position + Vector3.down);
            maximeMovable.LookAt(player.transform.position);
            yield return new WaitForSeconds(0.3f);
            playerMovable.LookAt(maximeMovable.transform.position);
            yield return new WaitForSeconds(0.2f);
            yield return new WaitWhile(() => luckyMovable.isMoving);

            wait = true;
            RpgManager.Instance.discussionInterface.SetImage(false, maximeSprite);
            player.Dialog(false, "Maxime : Merci de m'avoir libéré. Tu es exceptionnelle.", () => wait = false);
            yield return new WaitWhile(() => wait);

            wait = true;
            player.Dialog(true, "Morgane : Oh, Chaton !", () => wait = false);
            yield return new WaitWhile(() => wait);

            wait = true;
            RpgManager.Instance.discussionInterface.SetImage(false, maximeSprite);
            player.Dialog(false, "Maxime : Que dis-tu de t'installer ici ? On y est super bien, il y a de la place pour tout le monde.", () => wait = false);
            yield return new WaitWhile(() => wait);

            wait = true;
            RpgManager.Instance.discussionInterface.SetImage(false, alteaSprite);
            player.Dialog(false, "Altea : Ce serait un honneur de vivre ici avec vous. Il me faura juste un temps d'adaptation.", () => wait = false);
            yield return new WaitWhile(() => wait);

            luckyMovable.MoveTo(luckyMovable.transform.position + Vector3.up);

            wait = true;
            RpgManager.Instance.discussionInterface.SetImage(false, orionSprite);
            player.Dialog(false, "Orion : Ouais, ouais, c'est pas mal.", () => wait = false);
            yield return new WaitWhile(() => wait);

            wait = true;
            RpgManager.Instance.discussionInterface.SetImage(false, luckySprite);
            player.Dialog(false, "Lucky : Je vous innonderai d'amour !", () => wait = false);
            yield return new WaitWhile(() => wait);

            wait = true;
            player.Dialog(true, "Morgane : Arrêtez, je pleure déjà !", () => wait = false);
            yield return new WaitWhile(() => wait);

            wait = true;
            RpgManager.Instance.discussionInterface.SetImage(false, maximeSprite);
            player.Dialog(false, "Maxime : Morgane. Tu devrais faire le tour de la maison. Quelque chose t'attend à l'étage.", () => wait = false);
            yield return new WaitWhile(() => wait);

            wait = true;
            player.Dialog(true, "Morgane : A l'étage ?", () => wait = false);
            yield return new WaitWhile(() => wait);

            player.EndTalk();
            playerMovable.enabled = false;
            player.overrideMovement = false;
            RpgManager.SetKey(SaveKey.defeatedCerberus, 1);
            RpgManager.LoadScene("Montgeron", "Entrance");
            RpgManager.SaveGame("Entrance");
        }

        private IEnumerator OrionRoamCoroutine()
        {
            Vector3 pos1 = orionMovable.transform.position;
            Vector3 pos2 = orionMovable.transform.position + Vector3.down * 2.5f;

            while (true)
            {
                orionMovable.MoveTo(pos2, 1);
                yield return new WaitWhile(() => orionMovable.isMoving);
                yield return new WaitForSeconds(Random.Range(1.0f, 5.0f));
                orionMovable.MoveTo(pos1, 1);
                yield return new WaitWhile(() => orionMovable.isMoving);
                yield return new WaitForSeconds(Random.Range(1.0f, 5.0f));
            }
        }
    }
}