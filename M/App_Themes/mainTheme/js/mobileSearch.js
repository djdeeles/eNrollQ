function QueryStringeCevir(aranacakKelime)
{
    aranacakKelime = aranacakKelime.toLowerCase();
    aranacakKelime = aranacakKelime.replace(" ", "-");
    aranacakKelime = aranacakKelime.replace("ı", "i_");
    aranacakKelime = aranacakKelime.replace("ğ", "g_");
    aranacakKelime = aranacakKelime.replace("ü", "u_");
    aranacakKelime = aranacakKelime.replace("ş", "s_");
    aranacakKelime = aranacakKelime.replace("ö", "o_");
    aranacakKelime = aranacakKelime.replace("ç", "c_");
    aranacakKelime = aranacakKelime.replace("<", "");
    aranacakKelime = aranacakKelime.replace(">", "");
    aranacakKelime = aranacakKelime.replace(";", "");
    aranacakKelime = aranacakKelime.replace(",", "");
    aranacakKelime = aranacakKelime.replace("`", "");
    aranacakKelime = aranacakKelime.replace("'", "");
    aranacakKelime = aranacakKelime.replace("!", "");
    aranacakKelime = aranacakKelime.replace("/", "");
    aranacakKelime = aranacakKelime.replace("\\", "");
    aranacakKelime = aranacakKelime.replace("%", "");
    aranacakKelime = aranacakKelime.replace("^", "");
    aranacakKelime = aranacakKelime.replace("\"", "");
    return aranacakKelime;
}

function Search_m(event) {
    var key = event.keyCode;
    if (key == '13') {
        window.location = "ara-" + QueryStringeCevir($("#searchText").val());
        event.preventDefault();
    }
    return false;
}