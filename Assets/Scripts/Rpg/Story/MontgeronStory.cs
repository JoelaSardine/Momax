using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace rpg
{
    public class MontgeronStory : GameStory
    {
        public AudioClip bossMusic;

        public GameObject explosionPrefab;

        public Sprite maximeSprite;
        public Sprite alteaSprite;
        public Sprite orionSprite;

        public Teleporter entrance;
        public MovableEntity playerMovable;
        public MovableEntity alteaMovable;
        public MovableEntity orionMovable;
        public MovableEntity dogMovable;

        public Vector3 startPos;
        public float speed_walk = 1.0f;
        public float speed_run = 5.0f;
        public Vector3 morganePos1;
        public Vector3 morganePos2;
        public List<string> firstDialogue;
        public Vector3 catsPosition;
        public string talk_altea = "Altéa : Euh, Morgane... Je crois qu'il dit la vérité.";
        public string talk_orion = "Orion : La présence maléfique que je ressentais... C'est lui.";
        public string talk_orion_dog1 = "Orion : ENSIL ENSCI ! Le Démon qui terrorise Limoges depuis la nuit des temps !";
        public string talk_orion_dog2 = "Orion : Mais que fait-il ici ? Morgane, protège-nous !";
        public string talk_dog = "Mais... Qu'est-ce que...";

        private bool wait = false;
        private int counter = 0;

        private AudioSource audioSource;

        protected override IEnumerator Start()
        {
            audioSource = GetComponent<AudioSource>();

            yield return StartCoroutine(base.Start());

            if (RpgManager.GetKey(SaveKey.defeatedCerberus) != 1)
            {
                yield return StartCoroutine(FindMaxime());
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

            alteaMovable.transform.position = startPos;
            orionMovable.transform.position = startPos;
            dogMovable.transform.position = startPos;
            dogMovable.gameObject.SetActive(false);

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
            dogMovable.gameObject.SetActive(true);
            dogMovable.LookTowards(Vector3.up);
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
            dogMovable.MoveTo(morganePos1 + Vector3.down, speed_walk);
            yield return new WaitWhile(() => dogMovable.isMoving);
            
            TransitionBattle trbattle = TransitionBattle.Instance;
            trbattle.onClosureFinished = () => { SceneManager.LoadScene("PokemonBattle", LoadSceneMode.Additive); };
            trbattle.StartSpiralCoroutine();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.F10))
            {
                audioSource.Stop();
                Instantiate(explosionPrefab);
            }
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
        }
    }
}