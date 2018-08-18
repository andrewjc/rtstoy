using UnityEngine;

namespace Game.Render
{
    /// <summary>
    /// Encapsulates mouse movement for RtsCamera.
    /// </summary>
    [AddComponentMenu("Camera-Control/RtsCamera-Mouse")]
    public class RtsCameraTouch : MonoBehaviour
    {
        public string HorizontalInputAxis = "Horizontal";
        public string VerticalInputAxis = "Vertical";

        //

        private RtsCamera _rtsCamera;
        private Vector2 touchStartPos;
        private bool isDragging;
        private bool hasMovement;
        private float speed = 0.3f;

        //

        protected void Reset()
        {
        
        }

        protected void Start()
        {
            _rtsCamera = gameObject.GetComponent<RtsCamera>();
        }

        protected void Update()
        {
            if (_rtsCamera == null)
                return; // no camera, bail!

            if (Input.touchCount == 1)
            {
                // Drag around with finger
                Vector2 touchPosition = Input.GetTouch(0).position;
                if (touchStartPos == Vector2.zero)
                {
                    touchStartPos = touchPosition;
                }

                if (Vector2.Distance(touchPosition, touchStartPos) > 0.01f)
                {

                    // set drag flag on
                    if (!isDragging)
                    {
                        isDragging = true;

                    }

                    if (isDragging)
                    {

                        var h = touchPosition.x - touchStartPos.x;
                        if (Mathf.Abs(h) > 0.001f)
                        {
                            hasMovement = true;
                            _rtsCamera.AddToPosition(h*speed*Time.deltaTime, 0, 0);
                        }

                        var v = touchPosition.y - touchStartPos.y;
                        if (Mathf.Abs(v) > 0.001f)
                        {
                            hasMovement = true;
                            _rtsCamera.AddToPosition(0, 0, v*speed*Time.deltaTime);
                        }
                    }
                }

            }

            else
            {
                touchStartPos = Vector2.zero;
                isDragging = false;
            }
        }
    }
}
