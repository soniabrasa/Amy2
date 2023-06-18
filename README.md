# Amy

Se trata de programar en Unity un videojuego en 3D a llamado Amy a partir de los Assets proporcionados por mixamo.com.  

Se deben programar las mecánicas de control básico del personaje principal, usando la clase _CharacterController_, de la cámara.  


## Movimiento del personaje principal   

Programaremos el movimiento del personaje principal implementando una forma de control en 3ª persona bastante común en los videojuegos.   

Se debe crear un script de control de la Jugadora partiendo del ejemplo siguiente de la documentación de Unity:

https://docs.unity3d.com/ScriptReference/CharacterController.Move.html  

A partir de este código se debe conseguir un control básico el movimiento de **Amy** en 8 direcciones, cuatro correspondientes a los ejes X y Z y otras cuatro correspondientes a las direcciones intermedias y del control de salto.   


## Movimiento de la cámara   

Se debe crear, usando el paquete _Cinemachine_, un objeto de tipo `VirtualCamera` y configurarlo para que siga a la jugadora **Amy**.   

Se debe conseguir que una cámara reaccione a los movimientos de la protagonista.  

Se debe configurar el comportamiento de la cámara en aspectos como la velocidad a la que se acerca al personaje, el margen en el apuntado y el margen en el que el apuntado es progresivo frente a cuando es inmediato.   

## Animación de Amy  

Se debe crear un animador que permita que **Amy** ejecute las animaciones de los Assets de mixamo y que su código administre las transiciones adecuadas.  

## Control combinado del personaje y la cámara  

Para conseguir un comportamiento más flexible se debe probar:

- sofisticar el control del personaje mediante PlayerControl y seguir dejando que la cámara lo siga,

- o usar un control de cámara más sofisticado y aprovecharlo para controlar al personaje a partir de ella.  

Preferiblemente esta última.   

La idea es que la cámara pueda girar en torno al personaje, controlada por el joystick (o ratón), y que el personaje se mueva en la dirección hacia dónde apunta la cámara.   

Se puede lograr esto usando la cámara _FreeLookCamera_ de _Cinemachine_, haciendo algunos ajustes en el script de la jugadora.  

Este tipo de cámara virtual permite el movimiento en torno al personaje al que sigue, tanto girando horizontalmente en torno a él, como moviéndose verticalmente entre los límites marcados por los rig inferior y el superior.  

En el script de control de la jugadora se debe obtener una referencia a la cámara y modificar el vector _move_ del CharacterController para que su movimiento sea con referencia a la cámara.    

## Control del choque de la cámara con los muros  

Se debe añadir el componente _CinemachineCollider_.  

Este componente sirve para evitar que la cámara atraviese muros y otros objetos y nos encontremos mirando hacia el personaje con un muro en medio.  

El ajuste fundamental de este componente es la máscara de capas con las que se detecta la colisión y las etiquetas que se pueden excluir de la colisión.  

## La escapada de Amy  

A partir de esta configuración básica se irá programando un laberinto de detectores y puertas que lxs jugadorxs vivirán en primera persona como **Amy** y deben superar en tiempo y forma.  
