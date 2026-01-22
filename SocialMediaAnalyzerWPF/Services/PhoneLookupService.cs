using SocialMediaAnalyzerWPF.Models;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SocialMediaAnalyzerWPF.Services
{
    public class PhoneLookupService
    {
        private static readonly Dictionary<string, CountryInfo> CountryCodes = new Dictionary<string, CountryInfo>
        {
            {"1", new CountryInfo { Name = "United States", Code = "US", Continent = "North America", TimeZone = "America/New_York" }},
            {"7", new CountryInfo { Name = "Russia", Code = "RU", Continent = "Europe/Asia", TimeZone = "Europe/Moscow" }},
            {"20", new CountryInfo { Name = "Egypt", Code = "EG", Continent = "Africa", TimeZone = "Africa/Cairo" }},
            {"27", new CountryInfo { Name = "South Africa", Code = "ZA", Continent = "Africa", TimeZone = "Africa/Johannesburg" }},
            {"30", new CountryInfo { Name = "Greece", Code = "GR", Continent = "Europe", TimeZone = "Europe/Athens" }},
            {"31", new CountryInfo { Name = "Netherlands", Code = "NL", Continent = "Europe", TimeZone = "Europe/Amsterdam" }},
            {"32", new CountryInfo { Name = "Belgium", Code = "BE", Continent = "Europe", TimeZone = "Europe/Brussels" }},
            {"33", new CountryInfo { Name = "France", Code = "FR", Continent = "Europe", TimeZone = "Europe/Paris" }},
            {"34", new CountryInfo { Name = "Spain", Code = "ES", Continent = "Europe", TimeZone = "Europe/Madrid" }},
            {"36", new CountryInfo { Name = "Hungary", Code = "HU", Continent = "Europe", TimeZone = "Europe/Budapest" }},
            {"39", new CountryInfo { Name = "Italy", Code = "IT", Continent = "Europe", TimeZone = "Europe/Rome" }},
            {"40", new CountryInfo { Name = "Romania", Code = "RO", Continent = "Europe", TimeZone = "Europe/Bucharest" }},
            {"41", new CountryInfo { Name = "Switzerland", Code = "CH", Continent = "Europe", TimeZone = "Europe/Zurich" }},
            {"43", new CountryInfo { Name = "Austria", Code = "AT", Continent = "Europe", TimeZone = "Europe/Vienna" }},
            {"44", new CountryInfo { Name = "United Kingdom", Code = "GB", Continent = "Europe", TimeZone = "Europe/London" }},
            {"45", new CountryInfo { Name = "Denmark", Code = "DK", Continent = "Europe", TimeZone = "Europe/Copenhagen" }},
            {"46", new CountryInfo { Name = "Sweden", Code = "SE", Continent = "Europe", TimeZone = "Europe/Stockholm" }},
            {"47", new CountryInfo { Name = "Norway", Code = "NO", Continent = "Europe", TimeZone = "Europe/Oslo" }},
            {"48", new CountryInfo { Name = "Poland", Code = "PL", Continent = "Europe", TimeZone = "Europe/Warsaw" }},
            {"49", new CountryInfo { Name = "Germany", Code = "DE", Continent = "Europe", TimeZone = "Europe/Berlin" }},
            {"51", new CountryInfo { Name = "Peru", Code = "PE", Continent = "South America", TimeZone = "America/Lima" }},
            {"52", new CountryInfo { Name = "Mexico", Code = "MX", Continent = "North America", TimeZone = "America/Mexico_City" }},
            {"53", new CountryInfo { Name = "Cuba", Code = "CU", Continent = "North America", TimeZone = "America/Havana" }},
            {"54", new CountryInfo { Name = "Argentina", Code = "AR", Continent = "South America", TimeZone = "America/Argentina/Buenos_Aires" }},
            {"55", new CountryInfo { Name = "Brazil", Code = "BR", Continent = "South America", TimeZone = "America/Sao_Paulo" }},
            {"56", new CountryInfo { Name = "Chile", Code = "CL", Continent = "South America", TimeZone = "America/Santiago" }},
            {"57", new CountryInfo { Name = "Colombia", Code = "CO", Continent = "South America", TimeZone = "America/Bogota" }},
            {"58", new CountryInfo { Name = "Venezuela", Code = "VE", Continent = "South America", TimeZone = "America/Caracas" }},
            {"60", new CountryInfo { Name = "Malaysia", Code = "MY", Continent = "Asia", TimeZone = "Asia/Kuala_Lumpur" }},
            {"61", new CountryInfo { Name = "Australia", Code = "AU", Continent = "Oceania", TimeZone = "Australia/Sydney" }},
            {"62", new CountryInfo { Name = "Indonesia", Code = "ID", Continent = "Asia", TimeZone = "Asia/Jakarta" }},
            {"63", new CountryInfo { Name = "Philippines", Code = "PH", Continent = "Asia", TimeZone = "Asia/Manila" }},
            {"64", new CountryInfo { Name = "New Zealand", Code = "NZ", Continent = "Oceania", TimeZone = "Pacific/Auckland" }},
            {"65", new CountryInfo { Name = "Singapore", Code = "SG", Continent = "Asia", TimeZone = "Asia/Singapore" }},
            {"66", new CountryInfo { Name = "Thailand", Code = "TH", Continent = "Asia", TimeZone = "Asia/Bangkok" }},
            {"81", new CountryInfo { Name = "Japan", Code = "JP", Continent = "Asia", TimeZone = "Asia/Tokyo" }},
            {"82", new CountryInfo { Name = "South Korea", Code = "KR", Continent = "Asia", TimeZone = "Asia/Seoul" }},
            {"84", new CountryInfo { Name = "Vietnam", Code = "VN", Continent = "Asia", TimeZone = "Asia/Ho_Chi_Minh" }},
            {"86", new CountryInfo { Name = "China", Code = "CN", Continent = "Asia", TimeZone = "Asia/Shanghai" }},
            {"90", new CountryInfo { Name = "Turkey", Code = "TR", Continent = "Europe/Asia", TimeZone = "Europe/Istanbul" }},
            {"91", new CountryInfo { Name = "India", Code = "IN", Continent = "Asia", TimeZone = "Asia/Kolkata" }},
            {"92", new CountryInfo { Name = "Pakistan", Code = "PK", Continent = "Asia", TimeZone = "Asia/Karachi" }},
            {"93", new CountryInfo { Name = "Afghanistan", Code = "AF", Continent = "Asia", TimeZone = "Asia/Kabul" }},
            {"94", new CountryInfo { Name = "Sri Lanka", Code = "LK", Continent = "Asia", TimeZone = "Asia/Colombo" }},
            {"95", new CountryInfo { Name = "Myanmar", Code = "MM", Continent = "Asia", TimeZone = "Asia/Yangon" }},
            {"98", new CountryInfo { Name = "Iran", Code = "IR", Continent = "Asia", TimeZone = "Asia/Tehran" }},
            {"212", new CountryInfo { Name = "Morocco", Code = "MA", Continent = "Africa", TimeZone = "Africa/Casablanca" }},
            {"213", new CountryInfo { Name = "Algeria", Code = "DZ", Continent = "Africa", TimeZone = "Africa/Algiers" }},
            {"216", new CountryInfo { Name = "Tunisia", Code = "TN", Continent = "Africa", TimeZone = "Africa/Tunis" }},
            {"218", new CountryInfo { Name = "Libya", Code = "LY", Continent = "Africa", TimeZone = "Africa/Tripoli" }},
            {"220", new CountryInfo { Name = "Gambia", Code = "GM", Continent = "Africa", TimeZone = "Africa/Banjul" }},
            {"221", new CountryInfo { Name = "Senegal", Code = "SN", Continent = "Africa", TimeZone = "Africa/Dakar" }},
            {"222", new CountryInfo { Name = "Mauritania", Code = "MR", Continent = "Africa", TimeZone = "Africa/Nouakchott" }},
            {"223", new CountryInfo { Name = "Mali", Code = "ML", Continent = "Africa", TimeZone = "Africa/Bamako" }},
            {"224", new CountryInfo { Name = "Guinea", Code = "GN", Continent = "Africa", TimeZone = "Africa/Conakry" }},
            {"225", new CountryInfo { Name = "Ivory Coast", Code = "CI", Continent = "Africa", TimeZone = "Africa/Abidjan" }},
            {"226", new CountryInfo { Name = "Burkina Faso", Code = "BF", Continent = "Africa", TimeZone = "Africa/Ouagadougou" }},
            {"227", new CountryInfo { Name = "Niger", Code = "NE", Continent = "Africa", TimeZone = "Africa/Niamey" }},
            {"228", new CountryInfo { Name = "Togo", Code = "TG", Continent = "Africa", TimeZone = "Africa/Lome" }},
            {"229", new CountryInfo { Name = "Benin", Code = "BJ", Continent = "Africa", TimeZone = "Africa/Porto-Novo" }},
            {"230", new CountryInfo { Name = "Mauritius", Code = "MU", Continent = "Africa", TimeZone = "Indian/Mauritius" }},
            {"231", new CountryInfo { Name = "Liberia", Code = "LR", Continent = "Africa", TimeZone = "Africa/Monrovia" }},
            {"232", new CountryInfo { Name = "Sierra Leone", Code = "SL", Continent = "Africa", TimeZone = "Africa/Freetown" }},
            {"233", new CountryInfo { Name = "Ghana", Code = "GH", Continent = "Africa", TimeZone = "Africa/Accra" }},
            {"234", new CountryInfo { Name = "Nigeria", Code = "NG", Continent = "Africa", TimeZone = "Africa/Lagos" }},
            {"235", new CountryInfo { Name = "Chad", Code = "TD", Continent = "Africa", TimeZone = "Africa/Ndjamena" }},
            {"236", new CountryInfo { Name = "Central African Republic", Code = "CF", Continent = "Africa", TimeZone = "Africa/Bangui" }},
            {"237", new CountryInfo { Name = "Cameroon", Code = "CM", Continent = "Africa", TimeZone = "Africa/Douala" }},
            {"238", new CountryInfo { Name = "Cape Verde", Code = "CV", Continent = "Africa", TimeZone = "Atlantic/Cape_Verde" }},
            {"239", new CountryInfo { Name = "São Tomé and Príncipe", Code = "ST", Continent = "Africa", TimeZone = "Africa/Sao_Tome" }},
            {"240", new CountryInfo { Name = "Equatorial Guinea", Code = "GQ", Continent = "Africa", TimeZone = "Africa/Malabo" }},
            {"241", new CountryInfo { Name = "Gabon", Code = "GA", Continent = "Africa", TimeZone = "Africa/Libreville" }},
            {"242", new CountryInfo { Name = "Republic of the Congo", Code = "CG", Continent = "Africa", TimeZone = "Africa/Brazzaville" }},
            {"243", new CountryInfo { Name = "Democratic Republic of the Congo", Code = "CD", Continent = "Africa", TimeZone = "Africa/Kinshasa" }},
            {"244", new CountryInfo { Name = "Angola", Code = "AO", Continent = "Africa", TimeZone = "Africa/Luanda" }},
            {"245", new CountryInfo { Name = "Guinea-Bissau", Code = "GW", Continent = "Africa", TimeZone = "Africa/Bissau" }},
            {"246", new CountryInfo { Name = "British Indian Ocean Territory", Code = "IO", Continent = "Asia", TimeZone = "Indian/Chagos" }},
            {"248", new CountryInfo { Name = "Seychelles", Code = "SC", Continent = "Africa", TimeZone = "Indian/Mahe" }},
            {"249", new CountryInfo { Name = "Sudan", Code = "SD", Continent = "Africa", TimeZone = "Africa/Khartoum" }},
            {"250", new CountryInfo { Name = "Rwanda", Code = "RW", Continent = "Africa", TimeZone = "Africa/Kigali" }},
            {"251", new CountryInfo { Name = "Ethiopia", Code = "ET", Continent = "Africa", TimeZone = "Africa/Addis_Ababa" }},
            {"252", new CountryInfo { Name = "Somalia", Code = "SO", Continent = "Africa", TimeZone = "Africa/Mogadishu" }},
            {"253", new CountryInfo { Name = "Djibouti", Code = "DJ", Continent = "Africa", TimeZone = "Africa/Djibouti" }},
            {"254", new CountryInfo { Name = "Kenya", Code = "KE", Continent = "Africa", TimeZone = "Africa/Nairobi" }},
            {"255", new CountryInfo { Name = "Tanzania", Code = "TZ", Continent = "Africa", TimeZone = "Africa/Dar_es_Salaam" }},
            {"256", new CountryInfo { Name = "Uganda", Code = "UG", Continent = "Africa", TimeZone = "Africa/Kampala" }},
            {"257", new CountryInfo { Name = "Burundi", Code = "BI", Continent = "Africa", TimeZone = "Africa/Bujumbura" }},
            {"258", new CountryInfo { Name = "Mozambique", Code = "MZ", Continent = "Africa", TimeZone = "Africa/Maputo" }},
            {"260", new CountryInfo { Name = "Zambia", Code = "ZM", Continent = "Africa", TimeZone = "Africa/Lusaka" }},
            {"261", new CountryInfo { Name = "Madagascar", Code = "MG", Continent = "Africa", TimeZone = "Indian/Antananarivo" }},
            {"262", new CountryInfo { Name = "Réunion", Code = "RE", Continent = "Africa", TimeZone = "Indian/Reunion" }},
            {"263", new CountryInfo { Name = "Zimbabwe", Code = "ZW", Continent = "Africa", TimeZone = "Africa/Harare" }},
            {"264", new CountryInfo { Name = "Namibia", Code = "NA", Continent = "Africa", TimeZone = "Africa/Windhoek" }},
            {"265", new CountryInfo { Name = "Malawi", Code = "MW", Continent = "Africa", TimeZone = "Africa/Blantyre" }},
            {"266", new CountryInfo { Name = "Lesotho", Code = "LS", Continent = "Africa", TimeZone = "Africa/Maseru" }},
            {"267", new CountryInfo { Name = "Botswana", Code = "BW", Continent = "Africa", TimeZone = "Africa/Gaborone" }},
            {"268", new CountryInfo { Name = "Eswatini", Code = "SZ", Continent = "Africa", TimeZone = "Africa/Mbabane" }},
            {"269", new CountryInfo { Name = "Comoros", Code = "KM", Continent = "Africa", TimeZone = "Indian/Comoro" }},
            {"290", new CountryInfo { Name = "Saint Helena", Code = "SH", Continent = "Africa", TimeZone = "Atlantic/St_Helena" }},
            {"291", new CountryInfo { Name = "Eritrea", Code = "ER", Continent = "Africa", TimeZone = "Africa/Asmara" }},
            {"297", new CountryInfo { Name = "Aruba", Code = "AW", Continent = "North America", TimeZone = "America/Aruba" }},
            {"298", new CountryInfo { Name = "Faroe Islands", Code = "FO", Continent = "Europe", TimeZone = "Atlantic/Faroe" }},
            {"299", new CountryInfo { Name = "Greenland", Code = "GL", Continent = "North America", TimeZone = "America/Godthab" }},
            {"350", new CountryInfo { Name = "Gibraltar", Code = "GI", Continent = "Europe", TimeZone = "Europe/Gibraltar" }},
            {"351", new CountryInfo { Name = "Portugal", Code = "PT", Continent = "Europe", TimeZone = "Europe/Lisbon" }},
            {"352", new CountryInfo { Name = "Luxembourg", Code = "LU", Continent = "Europe", TimeZone = "Europe/Luxembourg" }},
            {"353", new CountryInfo { Name = "Ireland", Code = "IE", Continent = "Europe", TimeZone = "Europe/Dublin" }},
            {"354", new CountryInfo { Name = "Iceland", Code = "IS", Continent = "Europe", TimeZone = "Atlantic/Reykjavik" }},
            {"355", new CountryInfo { Name = "Albania", Code = "AL", Continent = "Europe", TimeZone = "Europe/Tirane" }},
            {"356", new CountryInfo { Name = "Malta", Code = "MT", Continent = "Europe", TimeZone = "Europe/Malta" }},
            {"357", new CountryInfo { Name = "Cyprus", Code = "CY", Continent = "Europe", TimeZone = "Asia/Nicosia" }},
            {"358", new CountryInfo { Name = "Finland", Code = "FI", Continent = "Europe", TimeZone = "Europe/Helsinki" }},
            {"359", new CountryInfo { Name = "Bulgaria", Code = "BG", Continent = "Europe", TimeZone = "Europe/Sofia" }},
            {"370", new CountryInfo { Name = "Lithuania", Code = "LT", Continent = "Europe", TimeZone = "Europe/Vilnius" }},
            {"371", new CountryInfo { Name = "Latvia", Code = "LV", Continent = "Europe", TimeZone = "Europe/Riga" }},
            {"372", new CountryInfo { Name = "Estonia", Code = "EE", Continent = "Europe", TimeZone = "Europe/Tallinn" }},
            {"373", new CountryInfo { Name = "Moldova", Code = "MD", Continent = "Europe", TimeZone = "Europe/Chisinau" }},
            {"374", new CountryInfo { Name = "Armenia", Code = "AM", Continent = "Europe/Asia", TimeZone = "Asia/Yerevan" }},
            {"375", new CountryInfo { Name = "Belarus", Code = "BY", Continent = "Europe", TimeZone = "Europe/Minsk" }},
            {"376", new CountryInfo { Name = "Andorra", Code = "AD", Continent = "Europe", TimeZone = "Europe/Andorra" }},
            {"377", new CountryInfo { Name = "Monaco", Code = "MC", Continent = "Europe", TimeZone = "Europe/Monaco" }},
            {"378", new CountryInfo { Name = "San Marino", Code = "SM", Continent = "Europe", TimeZone = "Europe/San_Marino" }},
            {"380", new CountryInfo { Name = "Ukraine", Code = "UA", Continent = "Europe", TimeZone = "Europe/Kiev" }},
            {"381", new CountryInfo { Name = "Serbia", Code = "RS", Continent = "Europe", TimeZone = "Europe/Belgrade" }},
            {"382", new CountryInfo { Name = "Montenegro", Code = "ME", Continent = "Europe", TimeZone = "Europe/Podgorica" }},
            {"383", new CountryInfo { Name = "Kosovo", Code = "XK", Continent = "Europe", TimeZone = "Europe/Belgrade" }},
            {"385", new CountryInfo { Name = "Croatia", Code = "HR", Continent = "Europe", TimeZone = "Europe/Zagreb" }},
            {"386", new CountryInfo { Name = "Slovenia", Code = "SI", Continent = "Europe", TimeZone = "Europe/Ljubljana" }},
            {"387", new CountryInfo { Name = "Bosnia and Herzegovina", Code = "BA", Continent = "Europe", TimeZone = "Europe/Sarajevo" }},
            {"389", new CountryInfo { Name = "North Macedonia", Code = "MK", Continent = "Europe", TimeZone = "Europe/Skopje" }},
            {"420", new CountryInfo { Name = "Czech Republic", Code = "CZ", Continent = "Europe", TimeZone = "Europe/Prague" }},
            {"421", new CountryInfo { Name = "Slovakia", Code = "SK", Continent = "Europe", TimeZone = "Europe/Bratislava" }},
            {"423", new CountryInfo { Name = "Liechtenstein", Code = "LI", Continent = "Europe", TimeZone = "Europe/Vaduz" }},
            {"500", new CountryInfo { Name = "Falkland Islands", Code = "FK", Continent = "South America", TimeZone = "Atlantic/Stanley" }},
            {"501", new CountryInfo { Name = "Belize", Code = "BZ", Continent = "North America", TimeZone = "America/Belize" }},
            {"502", new CountryInfo { Name = "Guatemala", Code = "GT", Continent = "North America", TimeZone = "America/Guatemala" }},
            {"503", new CountryInfo { Name = "El Salvador", Code = "SV", Continent = "North America", TimeZone = "America/El_Salvador" }},
            {"504", new CountryInfo { Name = "Honduras", Code = "HN", Continent = "North America", TimeZone = "America/Tegucigalpa" }},
            {"505", new CountryInfo { Name = "Nicaragua", Code = "NI", Continent = "North America", TimeZone = "America/Managua" }},
            {"506", new CountryInfo { Name = "Costa Rica", Code = "CR", Continent = "North America", TimeZone = "America/Costa_Rica" }},
            {"507", new CountryInfo { Name = "Panama", Code = "PA", Continent = "North America", TimeZone = "America/Panama" }},
            {"508", new CountryInfo { Name = "Saint Pierre and Miquelon", Code = "PM", Continent = "North America", TimeZone = "America/Miquelon" }},
            {"509", new CountryInfo { Name = "Haiti", Code = "HT", Continent = "North America", TimeZone = "America/Port-au-Prince" }},
            {"590", new CountryInfo { Name = "Guadeloupe", Code = "GP", Continent = "North America", TimeZone = "America/Guadeloupe" }},
            {"591", new CountryInfo { Name = "Bolivia", Code = "BO", Continent = "South America", TimeZone = "America/La_Paz" }},
            {"592", new CountryInfo { Name = "Guyana", Code = "GY", Continent = "South America", TimeZone = "America/Guyana" }},
            {"593", new CountryInfo { Name = "Ecuador", Code = "EC", Continent = "South America", TimeZone = "America/Guayaquil" }},
            {"594", new CountryInfo { Name = "French Guiana", Code = "GF", Continent = "South America", TimeZone = "America/Cayenne" }},
            {"595", new CountryInfo { Name = "Paraguay", Code = "PY", Continent = "South America", TimeZone = "America/Asuncion" }},
            {"596", new CountryInfo { Name = "Martinique", Code = "MQ", Continent = "North America", TimeZone = "America/Martinique" }},
            {"597", new CountryInfo { Name = "Suriname", Code = "SR", Continent = "South America", TimeZone = "America/Paramaribo" }},
            {"598", new CountryInfo { Name = "Uruguay", Code = "UY", Continent = "South America", TimeZone = "America/Montevideo" }},
            {"599", new CountryInfo { Name = "Caribbean Netherlands", Code = "BQ", Continent = "North America", TimeZone = "America/Curacao" }},
            {"670", new CountryInfo { Name = "East Timor", Code = "TL", Continent = "Asia", TimeZone = "Asia/Dili" }},
            {"672", new CountryInfo { Name = "Norfolk Island", Code = "NF", Continent = "Oceania", TimeZone = "Pacific/Norfolk" }},
            {"673", new CountryInfo { Name = "Brunei", Code = "BN", Continent = "Asia", TimeZone = "Asia/Brunei" }},
            {"674", new CountryInfo { Name = "Nauru", Code = "NR", Continent = "Oceania", TimeZone = "Pacific/Nauru" }},
            {"675", new CountryInfo { Name = "Papua New Guinea", Code = "PG", Continent = "Oceania", TimeZone = "Pacific/Port_Moresby" }},
            {"676", new CountryInfo { Name = "Tonga", Code = "TO", Continent = "Oceania", TimeZone = "Pacific/Tongatapu" }},
            {"677", new CountryInfo { Name = "Solomon Islands", Code = "SB", Continent = "Oceania", TimeZone = "Pacific/Guadalcanal" }},
            {"678", new CountryInfo { Name = "Vanuatu", Code = "VU", Continent = "Oceania", TimeZone = "Pacific/Efate" }},
            {"679", new CountryInfo { Name = "Fiji", Code = "FJ", Continent = "Oceania", TimeZone = "Pacific/Fiji" }},
            {"680", new CountryInfo { Name = "Palau", Code = "PW", Continent = "Oceania", TimeZone = "Pacific/Palau" }},
            {"681", new CountryInfo { Name = "Wallis and Futuna", Code = "WF", Continent = "Oceania", TimeZone = "Pacific/Wallis" }},
            {"682", new CountryInfo { Name = "Cook Islands", Code = "CK", Continent = "Oceania", TimeZone = "Pacific/Rarotonga" }},
            {"683", new CountryInfo { Name = "Niue", Code = "NU", Continent = "Oceania", TimeZone = "Pacific/Niue" }},
            {"686", new CountryInfo { Name = "Kiribati", Code = "KI", Continent = "Oceania", TimeZone = "Pacific/Kiritimati" }},
            {"687", new CountryInfo { Name = "New Caledonia", Code = "NC", Continent = "Oceania", TimeZone = "Pacific/Noumea" }},
            {"688", new CountryInfo { Name = "Tuvalu", Code = "TV", Continent = "Oceania", TimeZone = "Pacific/Funafuti" }},
            {"689", new CountryInfo { Name = "French Polynesia", Code = "PF", Continent = "Oceania", TimeZone = "Pacific/Tahiti" }},
            {"690", new CountryInfo { Name = "Tokelau", Code = "TK", Continent = "Oceania", TimeZone = "Pacific/Fakaofo" }},
            {"691", new CountryInfo { Name = "Federated States of Micronesia", Code = "FM", Continent = "Oceania", TimeZone = "Pacific/Pohnpei" }},
            {"692", new CountryInfo { Name = "Marshall Islands", Code = "MH", Continent = "Oceania", TimeZone = "Pacific/Majuro" }},
            {"850", new CountryInfo { Name = "North Korea", Code = "KP", Continent = "Asia", TimeZone = "Asia/Pyongyang" }},
            {"852", new CountryInfo { Name = "Hong Kong", Code = "HK", Continent = "Asia", TimeZone = "Asia/Hong_Kong" }},
            {"853", new CountryInfo { Name = "Macau", Code = "MO", Continent = "Asia", TimeZone = "Asia/Macau" }},
            {"855", new CountryInfo { Name = "Cambodia", Code = "KH", Continent = "Asia", TimeZone = "Asia/Phnom_Penh" }},
            {"856", new CountryInfo { Name = "Laos", Code = "LA", Continent = "Asia", TimeZone = "Asia/Vientiane" }},
            {"880", new CountryInfo { Name = "Bangladesh", Code = "BD", Continent = "Asia", TimeZone = "Asia/Dhaka" }},
            {"886", new CountryInfo { Name = "Taiwan", Code = "TW", Continent = "Asia", TimeZone = "Asia/Taipei" }},
            {"960", new CountryInfo { Name = "Maldives", Code = "MV", Continent = "Asia", TimeZone = "Indian/Maldives" }},
            {"961", new CountryInfo { Name = "Lebanon", Code = "LB", Continent = "Asia", TimeZone = "Asia/Beirut" }},
            {"962", new CountryInfo { Name = "Jordan", Code = "JO", Continent = "Asia", TimeZone = "Asia/Amman" }},
            {"963", new CountryInfo { Name = "Syria", Code = "SY", Continent = "Asia", TimeZone = "Asia/Damascus" }},
            {"964", new CountryInfo { Name = "Iraq", Code = "IQ", Continent = "Asia", TimeZone = "Asia/Baghdad" }},
            {"965", new CountryInfo { Name = "Kuwait", Code = "KW", Continent = "Asia", TimeZone = "Asia/Kuwait" }},
            {"966", new CountryInfo { Name = "Saudi Arabia", Code = "SA", Continent = "Asia", TimeZone = "Asia/Riyadh" }},
            {"967", new CountryInfo { Name = "Yemen", Code = "YE", Continent = "Asia", TimeZone = "Asia/Aden" }},
            {"968", new CountryInfo { Name = "Oman", Code = "OM", Continent = "Asia", TimeZone = "Asia/Muscat" }},
            {"970", new CountryInfo { Name = "Palestine", Code = "PS", Continent = "Asia", TimeZone = "Asia/Gaza" }},
            {"971", new CountryInfo { Name = "United Arab Emirates", Code = "AE", Continent = "Asia", TimeZone = "Asia/Dubai" }},
            {"972", new CountryInfo { Name = "Israel", Code = "IL", Continent = "Asia", TimeZone = "Asia/Jerusalem" }},
            {"973", new CountryInfo { Name = "Bahrain", Code = "BH", Continent = "Asia", TimeZone = "Asia/Bahrain" }},
            {"974", new CountryInfo { Name = "Qatar", Code = "QA", Continent = "Asia", TimeZone = "Asia/Qatar" }},
            {"975", new CountryInfo { Name = "Bhutan", Code = "BT", Continent = "Asia", TimeZone = "Asia/Thimphu" }},
            {"976", new CountryInfo { Name = "Mongolia", Code = "MN", Continent = "Asia", TimeZone = "Asia/Ulaanbaatar" }},
            {"977", new CountryInfo { Name = "Nepal", Code = "NP", Continent = "Asia", TimeZone = "Asia/Kathmandu" }},
            {"992", new CountryInfo { Name = "Tajikistan", Code = "TJ", Continent = "Asia", TimeZone = "Asia/Dushanbe" }},
            {"993", new CountryInfo { Name = "Turkmenistan", Code = "TM", Continent = "Asia", TimeZone = "Asia/Ashgabat" }},
            {"994", new CountryInfo { Name = "Azerbaijan", Code = "AZ", Continent = "Europe/Asia", TimeZone = "Asia/Baku" }},
            {"995", new CountryInfo { Name = "Georgia", Code = "GE", Continent = "Europe/Asia", TimeZone = "Asia/Tbilisi" }},
            {"996", new CountryInfo { Name = "Kyrgyzstan", Code = "KG", Continent = "Asia", TimeZone = "Asia/Bishkek" }},
            {"998", new CountryInfo { Name = "Uzbekistan", Code = "UZ", Continent = "Asia", TimeZone = "Asia/Tashkent" }}
        };

        private static readonly Dictionary<string, string> UsaAreaCodes = new Dictionary<string, string>
        {
            {"201", "New Jersey"},
            {"202", "Washington D.C."},
            {"203", "Connecticut"},
            {"205", "Alabama"},
            {"206", "Washington"},
            {"207", "Maine"},
            {"208", "Idaho"},
            {"209", "California"},
            {"210", "Texas"},
            {"212", "New York"},
            {"213", "California"},
            {"214", "Texas"},
            {"215", "Pennsylvania"},
            {"216", "Ohio"},
            {"217", "Illinois"},
            {"218", "Minnesota"},
            {"219", "Indiana"},
            {"224", "Illinois"},
            {"225", "Louisiana"},
            {"228", "Mississippi"},
            {"229", "Georgia"},
            {"231", "Michigan"},
            {"234", "Ohio"},
            {"239", "Florida"},
            {"240", "Maryland"},
            {"248", "Michigan"},
            {"251", "Alabama"},
            {"252", "North Carolina"},
            {"253", "Washington"},
            {"254", "Texas"},
            {"256", "Alabama"},
            {"260", "Indiana"},
            {"262", "Wisconsin"},
            {"267", "Pennsylvania"},
            {"269", "Michigan"},
            {"270", "Kentucky"},
            {"272", "Pennsylvania"},
            {"274", "North Carolina"},
            {"276", "Virginia"},
            {"281", "Texas"},
            {"283", "Ohio"},
            {"301", "Maryland"},
            {"302", "Delaware"},
            {"303", "Colorado"},
            {"304", "West Virginia"},
            {"305", "Florida"},
            {"307", "Wyoming"},
            {"308", "Nebraska"},
            {"309", "Illinois"},
            {"310", "California"},
            {"312", "Illinois"},
            {"313", "Michigan"},
            {"314", "Missouri"},
            {"315", "New York"},
            {"316", "Kansas"},
            {"317", "Indiana"},
            {"318", "Louisiana"},
            {"319", "Iowa"},
            {"320", "Minnesota"},
            {"321", "Florida"},
            {"323", "California"},
            {"325", "Texas"},
            {"330", "Ohio"},
            {"331", "Illinois"},
            {"334", "Alabama"},
            {"336", "North Carolina"},
            {"337", "Louisiana"},
            {"339", "Massachusetts"},
            {"347", "New York"},
            {"351", "Massachusetts"},
            {"352", "Florida"},
            {"360", "Washington"},
            {"361", "Texas"},
            {"364", "Kentucky"},
            {"380", "Ohio"},
            {"385", "Utah"},
            {"386", "Florida"},
            {"401", "Rhode Island"},
            {"402", "Nebraska"},
            {"404", "Georgia"},
            {"405", "Oklahoma"},
            {"406", "Montana"},
            {"407", "Florida"},
            {"408", "California"},
            {"409", "Texas"},
            {"410", "Maryland"},
            {"412", "Pennsylvania"},
            {"413", "Massachusetts"},
            {"414", "Wisconsin"},
            {"415", "California"},
            {"417", "Missouri"},
            {"419", "Ohio"},
            {"423", "Tennessee"},
            {"424", "California"},
            {"425", "Washington"},
            {"430", "Texas"},
            {"432", "Texas"},
            {"434", "Virginia"},
            {"435", "Utah"},
            {"440", "Ohio"},
            {"442", "California"},
            {"443", "Maryland"},
            {"445", "Pennsylvania"},
            {"458", "Oregon"},
            {"469", "Texas"},
            {"470", "Georgia"},
            {"475", "Connecticut"},
            {"478", "Georgia"},
            {"479", "Arkansas"},
            {"480", "Arizona"},
            {"484", "Pennsylvania"},
            {"501", "Arkansas"},
            {"502", "Kentucky"},
            {"503", "Oregon"},
            {"504", "Louisiana"},
            {"505", "New Mexico"},
            {"507", "Minnesota"},
            {"508", "Massachusetts"},
            {"509", "Washington"},
            {"510", "California"},
            {"512", "Texas"},
            {"513", "Ohio"},
            {"515", "Iowa"},
            {"516", "New York"},
            {"517", "Michigan"},
            {"518", "New York"},
            {"520", "Arizona"},
            {"530", "California"},
            {"531", "Nebraska"},
            {"534", "Wisconsin"},
            {"539", "Oklahoma"},
            {"540", "Virginia"},
            {"541", "Oregon"},
            {"551", "New Jersey"},
            {"559", "California"},
            {"561", "Florida"},
            {"562", "California"},
            {"563", "Iowa"},
            {"567", "Ohio"},
            {"570", "Pennsylvania"},
            {"571", "Virginia"},
            {"573", "Missouri"},
            {"574", "Indiana"},
            {"575", "New Mexico"},
            {"580", "Oklahoma"},
            {"585", "New York"},
            {"586", "Michigan"},
            {"601", "Mississippi"},
            {"602", "Arizona"},
            {"603", "New Hampshire"},
            {"605", "South Dakota"},
            {"606", "Kentucky"},
            {"607", "New York"},
            {"608", "Wisconsin"},
            {"609", "New Jersey"},
            {"610", "Pennsylvania"},
            {"612", "Minnesota"},
            {"614", "Ohio"},
            {"615", "Tennessee"},
            {"616", "Michigan"},
            {"617", "Massachusetts"},
            {"618", "Illinois"},
            {"619", "California"},
            {"620", "Kansas"},
            {"623", "Arizona"},
            {"626", "California"},
            {"628", "California"},
            {"629", "Tennessee"},
            {"630", "Illinois"},
            {"631", "New York"},
            {"636", "Missouri"},
            {"641", "Iowa"},
            {"646", "New York"},
            {"650", "California"},
            {"651", "Minnesota"},
            {"657", "California"},
            {"660", "Missouri"},
            {"661", "California"},
            {"662", "Mississippi"},
            {"667", "Maryland"},
            {"669", "California"},
            {"678", "Georgia"},
            {"681", "West Virginia"},
            {"682", "Texas"},
            {"701", "North Dakota"},
            {"702", "Nevada"},
            {"703", "Virginia"},
            {"704", "North Carolina"},
            {"706", "Georgia"},
            {"707", "California"},
            {"708", "Illinois"},
            {"712", "Iowa"},
            {"713", "Texas"},
            {"714", "California"},
            {"715", "Wisconsin"},
            {"716", "New York"},
            {"717", "Pennsylvania"},
            {"718", "New York"},
            {"719", "Colorado"},
            {"720", "Colorado"},
            {"724", "Pennsylvania"},
            {"725", "Nevada"},
            {"727", "Florida"},
            {"730", "Illinois"},
            {"731", "Tennessee"},
            {"732", "New Jersey"},
            {"734", "Michigan"},
            {"740", "Ohio"},
            {"747", "California"},
            {"754", "Florida"},
            {"757", "Virginia"},
            {"760", "California"},
            {"762", "Georgia"},
            {"763", "Minnesota"},
            {"765", "Indiana"},
            {"769", "Mississippi"},
            {"770", "Georgia"},
            {"772", "Florida"},
            {"773", "Illinois"},
            {"774", "Massachusetts"},
            {"775", "Nevada"},
            {"779", "Illinois"},
            {"781", "Massachusetts"},
            {"785", "Kansas"},
            {"786", "Florida"},
            {"801", "Utah"},
            {"802", "Vermont"},
            {"803", "South Carolina"},
            {"804", "Virginia"},
            {"805", "California"},
            {"806", "Texas"},
            {"808", "Hawaii"},
            {"810", "Michigan"},
            {"812", "Indiana"},
            {"813", "Florida"},
            {"814", "Pennsylvania"},
            {"815", "Illinois"},
            {"816", "Missouri"},
            {"817", "Texas"},
            {"818", "California"},
            {"828", "North Carolina"},
            {"830", "Texas"},
            {"831", "California"},
            {"832", "Texas"},
            {"843", "South Carolina"},
            {"845", "New York"},
            {"847", "Illinois"},
            {"848", "New Jersey"},
            {"850", "Florida"},
            {"856", "New Jersey"},
            {"857", "Massachusetts"},
            {"858", "California"},
            {"859", "Kentucky"},
            {"860", "Connecticut"},
            {"862", "New Jersey"},
            {"863", "Florida"},
            {"864", "South Carolina"},
            {"865", "Tennessee"},
            {"870", "Arkansas"},
            {"872", "Illinois"},
            {"878", "Pennsylvania"},
            {"901", "Tennessee"},
            {"903", "Texas"},
            {"904", "Florida"},
            {"906", "Michigan"},
            {"907", "Alaska"},
            {"908", "New Jersey"},
            {"909", "California"},
            {"910", "North Carolina"},
            {"912", "Georgia"},
            {"913", "Kansas"},
            {"914", "New York"},
            {"915", "Texas"},
            {"916", "California"},
            {"917", "New York"},
            {"918", "Oklahoma"},
            {"919", "North Carolina"},
            {"920", "Wisconsin"},
            {"925", "California"},
            {"928", "Arizona"},
            {"931", "Tennessee"},
            {"934", "New York"},
            {"936", "Texas"},
            {"937", "Ohio"},
            {"938", "Alabama"},
            {"940", "Texas"},
            {"941", "Florida"},
            {"947", "Michigan"},
            {"949", "California"},
            {"951", "California"},
            {"952", "Minnesota"},
            {"954", "Florida"},
            {"956", "Texas"},
            {"970", "Colorado"},
            {"971", "Oregon"},
            {"972", "Texas"},
            {"973", "New Jersey"},
            {"978", "Massachusetts"},
            {"979", "Texas"},
            {"980", "North Carolina"},
            {"984", "North Carolina"},
            {"985", "Louisiana"},
            {"989", "Michigan"}
        };

        public async Task<PhoneNumberResult> LookupPhoneNumberAsync(string phoneNumber, string defaultRegion = "US")
        {
            return await Task.Run(() =>
            {
                var result = new PhoneNumberResult
                {
                    PhoneNumber = phoneNumber
                };

                try
                {
                    result.IsValid = ValidatePhoneNumber(phoneNumber);
                    
                    if (result.IsValid)
                    {
                        result.FormattedNumber = FormatPhoneNumber(phoneNumber);
                        
                        var digitsOnly = Regex.Replace(phoneNumber, @"[^\d]", "");
                        
                        result.Status = "Valid";
                        result.IsPossible = true;
                        
                        var countryInfo = DetermineCountryFromNumber(digitsOnly);
                        result.RegionCode = countryInfo?.Code ?? defaultRegion;
                        result.CountryName = countryInfo?.Name ?? "Unknown";
                        result.Continent = countryInfo?.Continent ?? "Unknown";
                        result.Location = countryInfo?.Name ?? "Unknown";
                        result.Timezone = countryInfo?.TimeZone ?? "Unknown";
                        
                        result.NumberType = DetermineNumberType(phoneNumber);
                        result.Carrier = DetermineCarrier(digitsOnly, countryInfo?.Code);
                        
                        if (countryInfo?.Code == "US" && digitsOnly.Length >= 10)
                        {
                            var areaCode = digitsOnly.Substring(digitsOnly.Length - 10, 3);
                            result.AreaCode = areaCode;
                            if (UsaAreaCodes.ContainsKey(areaCode))
                            {
                                result.Location = $"{UsaAreaCodes[areaCode]}, United States";
                            }
                        }
                        
                        result.Provider = DetermineCarrier(digitsOnly, countryInfo?.Code);
                    }
                    else
                    {
                        result.Status = "Invalid Format";
                        result.ErrorMessage = "Phone number does not match standard formats";
                    }
                }
                catch (Exception ex)
                {
                    result.IsValid = false;
                    result.ErrorMessage = ex.Message;
                    result.Status = "Error";
                }

                return result;
            });
        }

        public bool ValidatePhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;

            var digitsOnly = Regex.Replace(phoneNumber, @"[^\d]", "");

            if (digitsOnly.Length < 7 || digitsOnly.Length > 15)
                return false;

            var internationalPattern = @"^\+[1-9]\d{1,14}$"; // International format
            var usPattern = @"^(\([0-9]{3}\)\s?|[0-9]{3}-)[0-9]{3}-[0-9]{4}$"; // US format
            var simplePattern = @"^[0-9]{7,15}$"; // Simple digit-only format
            
            return Regex.IsMatch(phoneNumber, internationalPattern) ||
                   Regex.IsMatch(phoneNumber, usPattern) ||
                   Regex.IsMatch(phoneNumber, simplePattern);
        }

        public string FormatPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return phoneNumber;

            var digitsOnly = Regex.Replace(phoneNumber, @"[^\d]", "");

            if (digitsOnly.Length == 11 && digitsOnly.StartsWith("1"))
            {
                return $"+{digitsOnly.Substring(0, 1)} ({digitsOnly.Substring(1, 3)}) {digitsOnly.Substring(4, 3)}-{digitsOnly.Substring(7, 4)}";
            }
            else if (digitsOnly.Length == 10)
            {
                return $"({digitsOnly.Substring(0, 3)}) {digitsOnly.Substring(3, 3)}-{digitsOnly.Substring(6, 4)}";
            }
            else if (phoneNumber.Trim().StartsWith("+"))
            {
                return phoneNumber.Trim();
            }
            else
            {
                return phoneNumber;
            }
        }

        private CountryInfo? DetermineCountryFromNumber(string digitsOnly)
        {
            for (int len = 4; len >= 1; len--)
            {
                if (digitsOnly.Length >= len)
                {
                    string countryCode = digitsOnly.Substring(0, len);
                    if (CountryCodes.ContainsKey(countryCode))
                    {
                        return CountryCodes[countryCode];
                    }
                }
            }
            
            return CountryCodes["1"];
        }

        private string DetermineNumberType(string phoneNumber)
        {
            var digitsOnly = Regex.Replace(phoneNumber, @"[^\d]", "");
            
            if (digitsOnly.Length >= 10)
            {
                if (digitsOnly.Length == 10 || (digitsOnly.Length == 11 && digitsOnly.StartsWith("1")))
                {
                    var areaCode = digitsOnly.Length == 11 ? digitsOnly.Substring(1, 3) : digitsOnly.Substring(0, 3);
                    
                    var mobileAreaCodes = new[] { "206", "212", "415", "650", "408", "651", "612", "917", "516", "631", "914", "845", "203", "917", "516", "631", "914", "845" };
                    if (mobileAreaCodes.Contains(areaCode))
                    {
                        return "Mobile";
                    }
                    
                    return "Mobile or Landline";
                }
                
                return "International";
            }
            
            return "Unknown";
        }

        private string DetermineCarrier(string digitsOnly, string? countryCode)
        {
            if (countryCode == "US")
            {
                if (digitsOnly.Length >= 10)
                {
                    var areaCode = digitsOnly.Length == 11 ? digitsOnly.Substring(1, 3) : digitsOnly.Substring(0, 3);
                    
                    var verizonAreas = new[] { "201", "212", "917", "516", "631", "718", "347", "929", "646", "215", "267", "484", "610", "412", "724", "414", "920", "262", "608", "206", "253", "425", "360" };
                    var attAreas = new[] { "404", "770", "678", "470", "670", "912", "478", "229", "225", "318", "337", "985", "228", "601", "662", "769", "334", "256", "938", "205", "901", "731", "870", "501", "479", "871", "417", "660", "816", "636", "314", "573", "667", "410", "301", "240", "202", "703", "571", "804", "434", "540", "828", "919", "984", "910", "980", "704", "828", "336", "919", "984", "910", "980", "704", "828", "336" };
                    var tmobileAreas = new[] { "206", "253", "425", "360", "509", "253", "425", "360", "509", "206", "253", "425", "360", "509", "213", "323", "424", "626", "661", "559", "760", "714", "951", "909", "951", "714", "909", "626", "661", "559", "760", "310", "424", "310", "424", "626", "661", "559", "760", "714", "951", "909", "213", "323", "424" };
                    
                    if (verizonAreas.Contains(areaCode)) return "Verizon";
                    if (attAreas.Contains(areaCode)) return "AT&T";
                    if (tmobileAreas.Contains(areaCode)) return "T-Mobile";
                }
                
                return "Major US Carrier";
            }
            else if (countryCode == "RU")
            {
                if (digitsOnly.Length >= 11)
                {
                    var operatorCode = digitsOnly.Length == 11 ? digitsOnly.Substring(1, 3) : digitsOnly.Length >= 4 ? digitsOnly.Substring(0, 3) : "";
                    
                    if (operatorCode.StartsWith("91") || operatorCode.StartsWith("92")) return "MTS";
                    if (operatorCode.StartsWith("95") || operatorCode.StartsWith("96") || operatorCode.StartsWith("97") || operatorCode.StartsWith("98")) return "Megafon";
                    if (operatorCode.StartsWith("90") || operatorCode.StartsWith("93")) return "Beeline";
                }
                
                return "Russian Operator";
            }
            else if (countryCode == "GB")
            {
                if (digitsOnly.Length >= 11)
                {
                    var operatorCode = digitsOnly.Length == 11 ? digitsOnly.Substring(1, 4) : digitsOnly.Length >= 5 ? digitsOnly.Substring(0, 4) : "";
                    
                    if (operatorCode.StartsWith("77")) return "O2";
                    if (operatorCode.StartsWith("78")) return "O2";
                    if (operatorCode.StartsWith("79")) return "O2";
                    if (operatorCode.StartsWith("74")) return "O2";
                    if (operatorCode.StartsWith("75")) return "Vodafone";
                    if (operatorCode.StartsWith("77")) return "Three";
                    if (operatorCode.StartsWith("78")) return "Three";
                }
                
                return "UK Operator";
            }
            else if (countryCode == "DE")
            {
                if (digitsOnly.Length >= 11)
                {
                    var operatorCode = digitsOnly.Length == 11 ? digitsOnly.Substring(1, 3) : digitsOnly.Length >= 4 ? digitsOnly.Substring(0, 3) : "";
                    
                    if (operatorCode == "151" || operatorCode == "160" || operatorCode == "170") return "Telekom";
                    if (operatorCode == "152" || operatorCode == "162" || operatorCode == "171" || operatorCode == "172") return "Vodafone";
                    if (operatorCode == "159" || operatorCode == "163" || operatorCode == "175" || operatorCode == "176") return "O2";
                }
                
                return "German Operator";
            }
            
            return "Local Carrier";
        }
    }

    internal class CountryInfo
    {
        public string Name { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string Continent { get; set; } = string.Empty;
        public string TimeZone { get; set; } = string.Empty;
    }
}