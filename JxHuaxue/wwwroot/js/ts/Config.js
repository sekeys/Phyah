require.config({
    //By default load any module IDs from js/lib
    baseUrl: '/js/ts/',
    //except, if the module ID starts with "app",
    //load it from the js/app directory. paths
    //config is relative to the baseUrl, and
    //never includes a ".js" extension since
    //the paths config could be for a directory.
    paths: {
        box: 'Box',
        color: 'color',
        data: "data",
        emitter:"Emitter",
        style: "style",
        Box: 'Box',
        xhr: "xhr",
        Xhr: "xhr",
        XHR: "xhr",
        Color: 'color',
        Data: "data",
        Emitter: "Emitter",
        Style: "style",
        "Zonic.base": "zonic.base",
        "zonic.base": "zonic.base"

    }
});

if (!("exports" in window)) {
    zonic=window.exports  = {}; 
}