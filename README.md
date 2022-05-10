# Map Story - An AR location-based story/game engine for Unity

Map Story is an Augmented Reality location story/game engine available as a Unity project. Two versions of the project are available for download, each set up to  build for either iOS or Android mobile phones, for devices capable of supporting AR through the phone camera. This is all iPhones (6S and later) and most non-lite models of Android handsets.

The story engine allows you to quickly create a location-based experience for use anywhere, with the app detecting suitable nearby sites to visit and incorporate as desired. The app will continue automtatically on arriving at each real world location shown on a local map, asking the user to interact with virtual models overlaid on the surroundings using AR, before continueing on to the next site. The engine offers a template to personalise according to your own story or game, adding appropraite virtual models for each location visited, alongside scripts representing your desired interactions with them.

Each of the GameObjects is numbered to help you set up the project in the Inspector window, in case it is not correctly set up when opening the **MapStory** project scene that contains the Map Story app template.

## Table of contents
1. [Quick start guide](#quickstart)
2. [Example gameplay](#gameplay)
3. [Sections of the story engine](#sections)
4. [Making your own location-based AR story](#makingyourownstory)
5. [Creating your own AR object interactions](#ARinteractions)
6. [Building your project](#buildingyourproject)
7. [Acknowledgements](#acknowledgements)
8. [Contact](#contact)

## Quick start guide <a name="quickstart"></a>
Before opening the project make sure that you have the required plugins installed and are using a suitable version of Unity. These are outlined in [Before starting](#sections).
Despite the Map Story engine just consisting of a series of blank templates for each page of the app, a finished app can be built and uploaded to a mobile phone quickly to demonstrate the app's features. The steps to achieve this are as follows:
1. Load the appropropriate iOS or Android Unity project according to which device you want to use the app on.
2. Enter your personal Mapbox ID under Unity's Mapbox menu (Mapbox -> Setup). If you do not have an ID already you can sign up for one for free through [Mapbox][https://account.mapbox.com/auth/signup/].
![Mapbox ID entry](QMapboxID.jpg)
3. Click on the **SearchLocalSites_12** GameObject in the Hierarchy window and locate its *SearchLocationsScript* in the Inspector. Enter:
&nbsp; - The number of different real world locations the user will be asked to visit during the story.
&nbsp; - The maximum play area specified as a radius in metres (a multiple of 50), around the user's starting location.
&nbsp; - Your personal Google Places key to connect to the API to search for local Points Of Interest (POIs). If you do not have one already you can sign up at [Google Maps][https://developers.google.com/maps]. Make sure that the Places API is added to the Google project associated with the key.
![Searching Local Sites](QSearchSites.jpg)
4. Select the **StoryManager_15** GameObject and its *StoryManager* script in the Inspector. You have the option of leaving the *Guided Story* variable checked or unchecked. By ticking the *Guided Story* variable the user will be guided to each POI on a local map based on the shortest walking route between the locations selected for them to visit. If the variable is unchecked, the user will be able to select which POI to walk to next from a local map.
![Story Manager Object](QStoryManager.jpg)
5. Find the **SiteAudio_17** GameObject in the Hierarchy which contains the audio tracks as child gameObjects, that will play as the user is walking to each POI. The number of children must match the number of POIs to be visited set in step 2, with each child GameObject having an associated *AudioSource* script attached to it. To get up and running quickly you can duplicate the attached **AudioTemplate** GameObject to the required number that contains a *jingle* audio track attached to it..
![Site Audio Object](QSiteAudio.jpg)
6. Locate the **ObjectsToPlace_16** GameObject, where each child represents one of 3D models to be overlaid on the user's surroundings through AR at each POI visited. The number of children must again match the number of POIs set in stage 2. To get started quickly you can duplicate the existing child object **3DCubePrefab** to the required number, though for your own projects you will want to create your own objects and interactions with them. The **3DCubePrefab** offers a simple example interaction through its attached *ObjectInteractionScript*.
![3D Objects to place](Q3DObjects.jpg)
7. Build the project using the existing project settings, though you may wish to assign your own choice of bundle identifier name for the project.
8. On starting the app on an appropriate device, you will need to allow access to both location services to identfy the user's position, as well as the phone's camera to enable the app's AR features. You will be prompted to do this when the app first starts.

## Example gameplay <a name="gameplay"></a>
Map Story offers a template for you to create your own location-based stories and games. The templates provided are described on the following pages, with this section outlining how gameplay currently operates for a user of the app.
1. On starting the app, it will search for a number of named locations around the user's detected GPS position. The identified Points Of Interest (POIs) will act as real world locations the user needs to visit, with their names able to be incoporated into the experience if desired.
2. The nearest of these these locations will be displayed on the map alongside a marker showing the user's current position. The user will also be provided with an optional route to walk to the target that will update as they move. On arriving at the target the story will continue automatically, with an option also provided to trigger the story or game to continue manually, in case the target is inaccessible due to obstacles in the real world preventing them getting close enough.
3. At each target location, the app will change to a view through the user's phone camera, where they will be asked to hold up their phone to place a virtual object on top of their surroundings through Augmented Reality. A bespoke inteaction be offered with the object, requiring the user to complete it in order to continue.
4. The user will then be required to visit the other locations in their local area, similarly placing and interacting with virtual objects. Map Story can either be setup to allow all the locations to be visited in the order that will minimise the overall walking distance, or allow the user to select the next location to visit from those identified on a map of their local area.
5. After visiting the pre-selected number of locations, an ending or closing screen is provided before exiting the app. 
![Example gameplay](Gameplay.jpg)

## Sections of the story engine <a name="sections"></a>
Each section of the Map Story engine is designed to be fully customisable according to your own story or game, including adding new sections desired. The current sections of the engine included are as folllows:

### Before starting
The Map Story Unity projects included for both iOS and Android were created in Unity 2019.2.11f1 so you will want to make sure you are running that or a newer version. You will also need to make sure you have the relevant packages installed before opening the project. Those required for the project are:
1. **ARFoundation** and **ARSubsystems** Version 3.1.3 - Allows the placement of Augmented Reality virtual objects, using SLAM tracking to align them to the physical surroundings.
2. **ARKit XR Plugin** Version 3.0.4 - AR implementation primarily used for iOS devices.
3. **ARCore XR Plugin** Version 3.1.4 - AR implementation primarily used for Android devices.
4. **XR Management** Version 3.0.6 - Manages XR plugin implementation.
4. **Json.NET for Unity** Version 10.0.302 - Provides a JSON framework used to receive data from the Google Places API.
Map Story is built using a modified version of the Mapbox for Unity SDK, which is included as part of the Unity projects. The Mapbox API is used for its map and navigation tools, so to use the finished app you will require a valid Mapbox ID, entered on the Mapbox setup menu added to the Unity menu (Mapbox -> Setup).
![Mapbox ID](MapboxID.jpg)
Your Mapbox ID can also be with the online Mapbox Studio software to create a customised map that best suits the theme of your story or game. After designing the look of your map using your public Mapbox ID, you will be provided with a Style URL that can be entered on the *Abstract Map* script of the **ARAlignedMap** GameObject, under the Image section, to add this map styling to your Map Story project.
![Mapbox Style](MapID.jpg)

### Title page
The **TitlePanel_1** GameObject contains a blank template for a title page. When the app is started a button will appear to search for a selected number of local named locations/POIs, that the user will be required to visit as part of the story. 
![Title Page](TitlePage.jpg)
The POIs are identified through the Google Places API, this search handled by the **SearchLocalSites_12** GameObject and its associated *SearchLocationsScript*. When creating your own experience you will need to set three values on this script in the Inspector. The first entry determines the number of real world locations that the user will be required to visit and so be identified through the Places API, selected according to their prominence rating and meeting other chosen criteria. This search will be performed in a selected radius around the GPS position where the user loads the app. To perform this search you will need a valid Google Places API key.

Variables to set:
- *No Locations To Visit* The number of real world locations to visit as part of your project.
- *Search Radius* The radius of the search area in metres to look for local sites. The number should be a multiple of 50.
- *API Key* Your Google Places API key must be entered in order to connect to the API and search for suitable map sites.
![Search Locations Object](SearchLocations.jpg)

### Instructions page
**InstructionsPanel_2** is the GameObject for a template to offer a user appropriate instructions for how to use the app. A basic set of instructions is included describing gameplay whereby a user's position is displayed on a map alongside a target location to walk to, with a suggesting walking route to it provided by the **Directions** GameObject. This navigation tool is taken from the Mapbox SDK and will update in real time as the user moves. The instructions page currently also makes mention of the map tools provided that include a button to recentre the camera on the user's location, as well as a button to trigger the story to continue manually in case it is difficult for them to reach the target location. Instructions for how to place virtual objects using AR are also provided here, though you will wish to customise these instructions according to your own project.
![Instruction Tools](MapTools.jpg)

### Warning page
By necessity the app will require the user to move around public spaces, and so it is important to offer a warning reminding them to take care if they are required to cross roads, as well as to respect other members of the public. The **WarningPanel_3** GameObject offers a screen to provide such a warning. Through offering audio whilst walking to the next target location, the app aims to keep the user safe by not requiring them to focus on their phone screen whilst moving, only doing so when stationary, at which point they may be asked to place and interact with virtual content.
![Warning Page](Warning.jpg)

### Story manager
The **StoryManager_15** GameObject and its *StoryManager* script deals with the set-up and progression between the different stages of the app, including checking when a user has arrived or triggered the next location. During the script's set-up phase, it examines the list of local locations identified by the Google API, and selects those to use for the story based on their metatags and sufficient spacing from other identified POIs. The names of each of the real world locations to be visited is also stored, so that they can be incorporated into story or game text displayed. After selecting the POIs to visit based on the *No Locations To Visit* variable set on the *SearchLocations* script, the shortest walk is plotted between them. This route and POI order will be offered if the *Guided Story* checkbox is set to true at the top of the **StoryManager_15** GameObject. Alternatively if this variable is left unchecked, the user will be able to select which location they want to visit next from the map. Selecting whether to leave this checked or unchecked will be driven by the type of story or game that you are looking to create.

Variables to set:
- *Guided Story* If checked, the user will be led between each map POI based on the shortest walk between them. If left unchecked, the user will be asked to select which target location on the map they want to walk to next.
![StoryManager Object](StoryManager.jpg)

### Story audio
An audio track will play as the user is walking to the next target location. These audio tracks must be listed as children of the **SiteAudio_17** GameObject, so each child must have an associated *AudioSource* script and audio track attached to it.
If the *GuidedStory* box is checked on the *StoryManager* script, each child GameObject audio track will be played in order, with the first track played whilst walking to the first POI and so on. If *GuidedStory* is unchecked, the order each audio track is played will vary according to the POI the user selects to visit. For example, if they select to visit the third POI, then the third audio track will be played as they walk to it.

Please Note:
- The number of child GameObjects on the **SiteAudio_17** GameObject must match the *No Locations To Visit* variable on the *SearchLocations* script for the app to work correctly. Each child also reqiures an *AudioSource* script and audio track to be set on it.
![Story Audio Object](StoryAudio.jpg)

### Map tools
A set of user tools for use with the map are provided. The map can be scrolled and pinched to zoom in or out. If the user loses track of their map location, a button is provided to recentre the map directly above the user's current real world position. A second button is also provided to trigger the story to continue manually in cases where they are unable to reach their target. This may occur because the target is inaccessible from being placed on private land or due to roadworks, or being placed somewhere the user would rather choose not to have to visit. If the story is continued manually, the user will just be asked to overlay the next virtual object at their current position. In the case where the *Guided Story* variable is set to false, the user will also be asked to select their next target location to visit from those remaining, by touching its marker on the map.
![Map Tools](MapTools.jpg)

### Location story details
After arriving at the next POI, a template **SiteInfoPanel_7** GameObject is provided that offers the user both an introduction to that section of the story or game, as well as an outro after completing the interaction with the associated 3D object, presented before moving to the next location. The information you want to display here will depend on the particular story or game you want to create, with it also possible to reference the name of the current real world POI, accessible through the *SiteName_Text_7_3* Text object. 
To summarise the *Intro_Text_7_4* Text will be displayed before placing the 3D object at the location being visited, with the *Outro_Text_7_5* Text then displayed after the interaction.
![Map Site Info](MapSite.jpg)

### Placing 3D objects
At each location visited, the user is asked to overlay a virtual model on top of their surroundings using AR, with the potential to code a bespoke interaction with the object before moving on to the next location. Placing these objects is handled by the *PlaceObject* script attached to the **PlaceObjectPanel_9** GameObject. After coding your own bespoke object interactions, you will need to modify the *ContinueApp* method in this script to reference your bespoke interaction scripts, rather than the *ObjectInteractionScript* currently referenced. This script offers a basic example interaction for those following the quick start guide.
The **ObjectsToPlace_16** GameObject contains the 3D models that the user will be asked to place at each site visited as its children. Please note that the top level of each child GameObject should contain a *Renderer* script used to display the object, as well as containing your bespoke interaction script. 
If the *GuidedStory* box is checked on the *StoryManager* script, each child GameObject will be placed in order, with the first child placed after visiting the first POI and so on. If *GuidedStory* is unchecked, the order each virtual object is placed will vary according to the POI the user selects to visit. For example, if they select to visit the third POI, the 3D model placed will be the third child listed under the **ObjectsToPlace_16** GameObject.
![Placing 3D Objects](PlaceObjects.jpg)

Please Note:
- The number of child GameObjects to **ObjectsToPlace_16** must match the *No Locations To Visit* variable on the *SearchLocations* script for the app to work. Any bespoke interactions you create with the objects must be also referenced accordingly in the *PlaceObject* script.

### Ending
After visiting all the pre-selected locations, the number set according to the *No Locations To Visit* variable on the *SearchLocations* script, the user will reach the end of the app. A simple ending template is provided with the **EndingPanel_10** GameObject and its associated *Ending* script. This basic script allows you to add an optional audio track to the *EndingAudio* variable that will play at this point before the user is asked to close the app. Again you will want to customise this page according to your particular project, perhaps linking to a credits page that lists those who were involved in making your project.
![Ending Page](EndingObject.jpg)

## Making your own location-based AR project <a name="makingyourownstory"></a>
Map Story contains the basic framework to allow you to create your own location-based story or game, that will direct users between a number of identified local POIs within a set *Search Radius* (a variable on the *SearchLocationsScript*), around the position where the user first loaded the app. The number of locations they will be required to visit can be set using the *No Locations To Visit*variable on the *SearchLocationsScript*). You will also want to decide whether the user should be guided between the locations and the 3D objects placed at each one in a pre-determined order, or whether the user is able to select which location to visit next and consequently the associated 3D object they will place there. This considerations will depend on the particular story or game you are looking to create with the option set via the *Guided Story* tickbox on the *StoryManager* script.

You will also want to consider whether you have adopted a consitent theme throughout your project in terms of the 3D objects selected to place, the choice of story, and the styling of the map and pages presented throughout the app. The map can be styled according to your theme using the online [Mapbox studio][https://www.mapbox.com/mapbox-studiosoftware] software and your Mapbox ID. Once you have created your own map styling you can use this within your project by selecting a Custom *Data Source*, and entering †he unique *Style URL* on the *Abstract Map* script of the **ARAlignedMap** GameObject, under the Image section.

The template provided for the app is only offered as a guide, you may wish to add further pages according to the particular project you are mlooking to create, with the scripts included easily adjustable to enable you to do this, with comments in the code describing the role of each variable and method included.
![Example apps](ExampleApps.jpg)

## Creating your own AR object interactions <a name="ARinteractions"></a>
As part of your own app you may want to allow the user to interact with the virtual objects they overlay on their surroundings at each location visited. Various possibilities for interacting with them are possible, such as tied to using the phone's touchscreen or identifying movement of its accelerometer. These interactions will depend on the particular 3D models you have chosen to place, in which case the relevant script will need to be enabled according to the particular object. 

A comment has been left where this may be done in the *PlaceObjects* script, replacing the call to enable the *ObjectInteractionScript* which is associated with the **3DCubePrefab** GameObject, for use with the quick start guide. This script demonstrates a simple interaction with the Rubik cube prefab. After placing it, the user is asked to spin the cube using the phone's touhscreen, which will cause the Rubik cube to become solved, each face becoming a single colour. The user is then asked to touch the red face of the cube to continue, returning to the current site's outro page, before directing them to the next location.

The type of interactions you can offer is only limited by your imagination and will depend on the particular story or game you are looking to create.
![Rubik cube interaction](RubikCube.jpg)

## Building your project <a name="buildingyourproject"></a>
The build settings (*Edit -> Project Settings...*) of both the iOS and Android versions of the Unity project have been set-up to allow you to quickly build each project for a suitable mobile phone capable of supporting AR through the phone camera. However, it is suggested that you set the Project Name (Project Settings... -> Player) and Bundle Identifier (Project Settings... -> Player -> Other according to your personal project.

Please Note:
If building to iOS you may be required to make the following change to the files built by Unity before opening it in Xcode. This is a recent bug, with the change outlined preventing the Xcode build failing with the following error:
'''
Cannot initialize a parameter of type 'id<NSCopying> _Nonnull' with an rvalue of type 'Class'
'''
To fix this in your project folder built from Unity identify the ARSessionNative.mm file (Libraries -> Mapbox -> UnityARKitPlugin -> Plugins -> iOS -> UnityARKit -> NativeInterface) and make the following changes in a text editor before saving, which both occur on a single line:
1. forKey:[ARPlaneAnchor class] should be changed to forKey:(id<NSCopying)[ARPlaneAnchor class]
2. forKey:[ARAnchor class] should be changed to forKey:(id<NSCopying)[ARAnchor class]


## Acknowledgements <a name="acknowledgements"></a>
Map Story is built on top of the Mapbox for Unity SDK, which is included as part of the Unity project. It also makes use of the Google Places API, with the example 3D model and audio files included as part of the Quick Start project, conisiting of the following resources used under a CC-BY license:
- Rubik cubes by krabz (Blend Swap)
- JinglePiano by Jonesbrt (freesound)

## Contact <a name="contact"></a>
If you have any questions or difficulties using Map Story, please feel free to get in contact and I will get back to you with a response. You can contact me at <mapstory2ar@gmail.com>.
