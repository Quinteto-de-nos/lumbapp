
char estado_puertos [11] = {"1111111111$"};
char prueba;
void setup() {


Serial.begin(9600);

}

void loop() {

Serial.println("1111111111$"); //todo en 0, el $ es para indicar que terminó el string

delay(500);

Serial.println("0111111111$"); //todo en 0, el $ es para indicar que terminó el string

delay(500);

Serial.println("0111110111$"); //todo en 0, el $ es para indicar que terminó el string

delay(500);

Serial.println("0111110111$"); //todo en 0, el $ es para indicar que terminó el string

delay(500);

Serial.println("0111110110$"); //hubo cambio, el $ es para indicar que terminó el string

delay(500);

Serial.println("0111110111$"); //no hubo cambio, el $ es para indicar que terminó el string

delay(500);

Serial.println("0111111111$"); //hubo cambio, el $ es para indicar que terminó el string

delay(500);

Serial.println("1111111111$"); //no hubo cambio, el $ es para indicar que terminó el string

delay(500);
}
