# Eltariff-API
_Specifikation för ett API för Elnätstariffer i Sverige._

__Notera att tillhandahållandet av data i API:t är i ett utvecklingsskede. Det är sannolikt inte bindande, används på egen risk och innebär således inte några skyldigheter för elnätsbolagen om inte annat meddelas. För mer information kontakta respektive nätbolag.__
# Dokumentation
Eltariff-API bygger på att elnätsoperatörer publicerar sina tariffer enligt en gemensam [API-specifikation](specification/gridtariffapi.json)
och registrerar sin lösning hos en gemensam katalogtjänst, vars API-specifikation finns [här](specification/catalogueapi.json)
Den som vill hämta tariff-information börjar med att anropa katalog-tjänsten med ett eller flera anläggnings-id för at få information om var tariff-information kan hämtas för respektive anläggnings-id.

![Basic onboarding sequence](doc/Eltariff_sequence_diagram.svg)

