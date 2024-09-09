using UnityEngine;
public abstract class InteractableObject : MonoBehaviour
{
    public bool isDebug = false;
    Material outline;   
    MeshRenderer meshRenderer;
    public bool is2DObject;
    
    public abstract void Interacted(); 
    public void Awake() {
        if(TryGetComponent<MeshRenderer>(out MeshRenderer meshRenderer)){
            this.meshRenderer = meshRenderer;
            outline = meshRenderer.materials[1];
        }
       
    }
    private void Start() {
        DisableOutline();        
    }
   public void DisableOutline(){
        outline.SetFloat("_Scale", 0f);
    }
    public void EnableOutline(){
        outline.SetFloat("_Scale", 1.125f);
   }
}