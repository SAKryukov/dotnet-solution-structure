// To detect invalid anchor targets, add:
//     <script src="prepare/validator.js"></script>
//
window.onload = () => {
    const errors = [];
    const anchors = document.getElementsByTagName("a");
    for (let anchor of anchors) {
        if (!anchor.hash) continue;
        //if (anchor.protocol == "http:" || anchor.protocol == "https:")
        //    continue;
        if (anchor.protocol != "file:")
            continue;
        element = document.getElementById(anchor.hash.replace("#", ""));
        if (!element)
            errors.push(anchor);

    } //loop
    if (errors.length > 0) {
        document.writeln(`<h1>Errors: invalid anchor targets:<h1>`);
        for(let anchor of errors)
            document.writeln(`<h3> ${anchor.hash}<dd>Text: "${anchor.textContent}"</dd></h3>`);
    } //if
} //window.onload