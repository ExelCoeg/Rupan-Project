using UnityEngine;
public abstract class InteractableObject : MonoBehaviour
{
    public string objectName;
    public bool isDebug = false;
    public bool isInteractable;
    Material outline;  
    public abstract void Interacted(); 
    public virtual void Awake() {
        if(TryGetComponent(out MeshRenderer meshRenderer)){
            for(int i = 0; i < meshRenderer.materials.Length; i++){
                if(meshRenderer.materials[i].name.Contains("Outline")){
                    outline = meshRenderer.materials[i];
                    break;
                }
            }
        }
    }

    private void Update() {
        if(!isInteractable && outline.GetFloat("_Scale") > 0){
            outline.SetFloat("_Scale", 0f);
        }
    }
   public void DisableOutline(){
        if(outline != null && isInteractable){
            outline.SetFloat("_Scale", 0f);
        }
    }
    public void EnableOutline(){
        if(outline != null && isInteractable){
            outline.SetFloat("_Scale", 1.025f);
        }
   }

   public void EnableInteractable(){
        print("EnableInteractable");
        isInteractable = true;     
   }
    public void DisableInteractable(){
        print("DisableInteractable");
        isInteractable = false;     
    }
}