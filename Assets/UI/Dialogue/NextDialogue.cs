//using UnityEngine;

//public class NextDialogue : MonoBehaviour
//{
//    int index = 2;

//    // Start is called once before the first execution of Update after the MonoBehaviour is created
//    void Start()
//    {
        
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (Input.GetMouseButtonDown(0) && transform.childCount > 1) {
//            if (PlayerMovement.dialogue)
//            {
//                transform.GetChild(index).gameObject.SetActive(true);
//                index++;
//                if (transform.childCount == index) {
//                    index = 2;
//                    PlayerMovement.dialogue = false;
//                }
//            }
//            else {
//                for (int i = 1; i < transform.childCount; i++)
//                {
//                    Destroy(transform.GetChild(i).gameObject);
//                }
//                gameObject.SetActive(false);
//            }
//        }
        
//    }
//}
