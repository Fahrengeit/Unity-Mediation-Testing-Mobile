# Unity-Mediation-Testing-Mobile

This is a repository for the Unity project that provides simple functions of loading and showing ads through Unity Mediation with room for expanding into other Mediations.

## Initialization

To get started with the application, follow the instructions below:

1. Clone this project and open it in the Unity Editor.
2. Choose your mobile platform (Android or iOS). This application was tested on iOS.
3. Setup your project for Unity Mediation, according to the checklist https://docs.unity.com/mediation/MediationSetupChecklist.html
4. Open the UnityMediationAdScene scene and navigate to AdController game object.
5. Update **Game ID** in **Unity Mediation Controller** with your Unity Mediation Game ID that's associated with this project.
6. Update **AdUnitId** in **InterstitialAdController** and **RewardedAdController** scripts with ad unit IDs that you should create in Dashboard.
7. Build.