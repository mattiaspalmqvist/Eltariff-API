# Eltariff-API
_Specifikation för ett API för Elnätstariffer i Sverige._

# Dokumentation
Eltariff-API bygger på att elnätsoperatörer publicerar sina tariffer enligt en gemensam [API-specifikation](specification/gridtariffapi.json)
och registrerar sin lösning hos en gemensam katalogtjänst, vars API-specifikation finns [här](Eltariff-catalogue-v0.6-openapi.json)
Den som vill hämta tariff-information börjar med att anropa katalog-tjänsten med ett eller flera anläggnings-id för at få information om var tariff-information kan hämtas för respektive anläggnings-id.

![Basic onboarding sequence](doc/Eltariff_sequence_diagram.svg)

