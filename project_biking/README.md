
# Let's go Biking

L'objectif de ce projet est de créer toutes les parties d'une application qui permettrait à l'utilisateur de trouver son chemin de n'importe quel endroit à n'importe quel autre endroit en utilisant autant que possible les vélos proposés par JC Decaux.

Ce projet est constitué de 4 sous-projets dont je vais vous présenter en détails leurs fonctionnement :
- WebProxyService (WCF Library)
- RoutingWithBikes (WCF Library)
- HeavyClient (Application console .Net Framework)
- LightWebClient (Interface web)

Mais avant ça, voici un rapide paragraphe sur comment faire fonctionner le projet.

## Faire fonctionner le projet

Dans un premier temps, il faut lancer le WebProxyService. Voici deux adresses pour tester ses deux services REST :
- http://localhost:8733/Lets_go_biking/WebProxyService/Service1/GetAllStations
- http://localhost:8733/Lets_go_biking/WebProxyService/Service1/GetStation?contract=lyon&number=2010

Ensuite, il faut lancer en parallèle RoutingWithBikes. RoutingWithBikes doit en fait faire des appels REST à WebProxyService mais par la suite, nous feront toujours des appels à RoutingWithBikes. Voici d'ailleurs quelques adresses qui vont vous permettre de tester facilement le projet :
- Appel avec des coordonnées : http://localhost:8734/Lets_go_biking/RoutingWithBikes/Service1/rest/GetRestStationsAndItinary?lat=43.644132925978056&lon=1.4323214205002373&lat2=43.56359172881835&lon2=1.4472526870014837
- Ici même position de départ et d'arrivé, doit nous dire qu'il vaut mieux y aller à pied : http://localhost:8734/Lets_go_biking/RoutingWithBikes/Service1/rest/GetRestStationsAndItinary?lat=43.56359172881835&lon=1.4472526870014837&lat2=43.56359172881835&lon2=1.4472526870014837
- Appel avec une adresse : http://localhost:8734/Lets_go_biking/RoutingWithBikes/Service1/rest/GetRestStationsAndItinary?lat=43.644132925978056&lon=1.4323214205002373&goalAdress=1%20Rue%20Claude-Marie%20Perroud%2C%2031000%20Toulouse

Nous pouvons également faire les mêmes requêtes mais recevoir les données sous forme de string en appelant plutôt ces services :
- Appel avec des coordonnées : http://localhost:8734/Lets_go_biking/RoutingWithBikes/Service1/rest/GetSoapStationsAndItinary?lat=43.644132925978056&lon=1.4323214205002373&lat2=43.56359172881835&lon2=1.4472526870014837
- Ici même position de départ et d'arrivé, doit nous dire qu'il vaut mieux y aller à pied : http://localhost:8734/Lets_go_biking/RoutingWithBikes/Service1/rest/GetSoapStationsAndItinary?lat=43.56359172881835&lon=1.4472526870014837&lat2=43.56359172881835&lon2=1.4472526870014837
- Appel avec une adresse : http://localhost:8734/Lets_go_biking/RoutingWithBikes/Service1/rest/GetSoapStationsAndItinary?lat=43.644132925978056&lon=1.4323214205002373&goalAdress=1%20Rue%20Claude-Marie%20Perroud%2C%2031000%20Toulouse
-
À noter qu'ici se sont que des appels REST, il y a le HeavyClient pour test des requêtes SOAP.
Pour tester mon LightClientWeb, il suffit juste d'ouvrir le index.html sur un navigateur. Il vous simule une postition sur Toulouse (cf: [ LightWebClient (Interface web)]( #LightWebClient (Interface web))).
Voici quelques valeurs que vous pouvez mettre dans les formulaires pour avoir des valeures cohérentes :
- latitude : 43.56359172881835
- longitude : 1.4472526870014837
- adresse : 1 Rue Claude-Marie Perroud, 31000 Toulouse

Maintenant merci de bien lire la suite pour bien comprendre le fonctionnement du projet et d'également comprendre mes choix vis-à-vis de celui-ci.

## WebProxyService (WCF Library)
L'objectif de cette partie est surtout de créer un système de cache pour éviter d'appeler sans cesse les APIs de JC Decaux. Cette bibliothèque fournit deux services REST.

GetAllStations() qui renvoie toute les stations de JC Decaux. Ces stations là sont stockés dans un cache qui expire au bout d'une semaine. À noter que les stations renvoyées par ce service ne contiennent pas la donnée du nombre de vélos disponibles pour éviter d'induire un utilisateur en erreur puisque cette donnée ne sera pas à jour.

Pour connaitre le nombre de vélos d'une station, il faut appeler l'autre service (GetStation(number, contrat)). Ici, avec le contrat et le numéro de la station en paramètre, le service renvoie les données d'une station en particulier. Ces données sont stockées dans un autre cache qui lui fait expirer ses éléments au bout de 2 minutes. Ainsi, le nombre de vélos pour une station est mis à jour régulièrement.

## RoutingWithBikes (WCF Library)
Le but de cette partie est de fournir un service qui renvoie l'itinéraire pour aller d'un point A à un point B en utilisant au maximum les vélos mis à disposition par JC Decaux.
Pour cela, le service prend en paramètre les coordonnées de départ ainsi que les corrdonnées d'arrivé (à noter qu'il est possible de fournir des adresses au lieu des coordonnées. Ces adresses sont alors converti en coordonnées par le programme en faisant appel à une API de OpenRouteServices).

Ensuite, pour la position de départ et d'arrivé, il faut trouver la station la plus proche ayant des vélos disponibles. Pour cela, je récupère dans un premier temps toutes les stations de JC Decaux en appeler mon service du WebProxyService. Puis, je filtre les stations par distance avant de les trier par la station la plusproche (à noter que par défaut, la distance qui sert à filter les stations est de 50km et que cette distance peut être choisi en paramètre du service). Avec une liste de stations filtrée et triée, j'énumère les stations, j'appelle GetStation du WebProxyService pour savoir le nombre de vélos disponibles et ce jusqu'à trouver une station qui a des vélos disponibles.

S'il n'y a pas de stations disponibles ou que la station la plus proche du point de départ est la même que la station la plus proche du point d'arrivé, le service retourne un résultant indiquant qu'il vaut mieux être à pied.
Si ce n'est pas le cas, il faut alors chercher les itinéraires pour aller du point de départ jusqu'à la station 1, de la station 1 à la station 2 et de la station 2 au point d'arrivé. Pour cela, je fait des appels à une API de OpenRouteServices en faisant attention d'indiquer le bon moyen de transport. Grâce à cela, notre service a toutes les données nécessaires et renvoie donc les deux stations ainsi que les itinéraires.

Le serveur est accessible en SOAP et REST. Il est juste important de noter que en réalité, j'ai créé deux services différents faisant la même chose. La seule différence est que l'un renvoie le JSON sous forme de string (GetSoapStationsAndItinary) alors que l'autre le renvoie sous forme de Stream (GetRestStationsAndItinary). Le JSON renvoyé par le Stream est bien plus facile à désérialiser que pour le string. Seulement, cela fonctionne que pour des appels REST. C'est pour cela que j'ai choisi de garder les deux, pour avoir un service facile d'utilisation pour le REST, et un autre qui fonctionne aussi bien avec des appels REST que SOAP.

## HeavyClient (Application console .Net Framework)

Le client lourd est un client en console. Il fait des appels en SOAP sur mon serveur et on constate qu'il reçoit bien les résultats. Il est juste important de noter que comme dit précédemment, il ne peut pas appeler le service qui retourne un Stream mais seulement celui qui retourne un string.

## LightWebClient (Interface web)

Le client léger est une interface web classqiue fait de JS, HTML et CSS sans aucun framework utilisé. Il fait des appels en REST à notre serveur et adapte les résultats pour qu'ils soient visible sur une carte.
À noter également que pour une question de test, le site simule la récupération de la position de l'utilisateur. Si nous voulons vraiment l'activer, il suffit juste de décommenter les lignes 62 et 63 du main.js.
