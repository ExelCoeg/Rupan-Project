using UnityEngine;
public abstract class InteractableObject : MonoBehaviour
{
    public bool isDebug = false;
    public bool canEnableOutline = true;
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
    // private void Start() {
    //     DisableOutline();        
    // }
   public void DisableOutline(){
        if(outline != null){
            outline.SetFloat("_Scale", 0f);
        }
    }
    public void EnableOutline(){
        if(outline != null){
            outline.SetFloat("_Scale", 1.025f);
        }
   }
}