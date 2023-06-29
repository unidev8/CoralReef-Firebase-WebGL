mergeInto(LibraryManager.library, {

    showShop: function (){
        window.showShopView();
    },

    userLogin: function (){
        window.toggleUserLogin();
    },

    dologinAction: function (){
        window.doLogin();
    },

    getUrlParams: function () {
        var params = new Proxy(new URLSearchParams(window.location.search), {
            get: (searchParams, prop) => searchParams.get(prop),
        });    
        return params;
    },

    getUrlParam: function (name) {
        return (new URL(window.location.href).searchParams).get(name);
    },

    getSearchParams: function () {
        var returnStr = window.location.search;
        var bufferSize = lengthBytesUTF8(returnStr) + 1;
        var buffer = _malloc(bufferSize);
        stringToUTF8(returnStr, buffer, bufferSize);
        return buffer;
    },

    getUrlReefId: function () {
        return window.UrlGetParams.reef_id;
    },


    getAllAssets: function (reef_id) {
        console.log('Running getAllAsset() functions with the reef_if: '+reef_id, Date.now());

        try {
            console.log('Do we recognise Firebase? ',firebase);

            firebase.firestore().collection('reef_id').doc(reef_id).collection('reef_assets').get().then(function (querySnapshot) {
                console.log('Got the doc(s) from this query: ', querySnapshot.size);

                var docs = {};
                var i = 1;
                querySnapshot.forEach(function(doc) {
                    console.log('ADDED NEW ASSET DOC TO docs ARRAY: ', i, doc.id, doc.data());
                    docs[doc.id] = doc.data();

                    if(i == querySnapshot.size){
                        return JSON.stringify(docs); 
                    }else {
                        i++;
                    }
                    
                });
                
                //window.unityInstance.SendMessage(parsedObjectName, parsedCallback, JSON.stringify(docs));
            }).catch(function(error) {
                //window.unityInstance.SendMessage(parsedObjectName, parsedFallback, JSON.stringify(error, Object.getOwnPropertyNames(error)));
                console.log('ERROR IN FIRESTORE QUERY: ', error);
                return JSON.stringify(error);
            });

        } catch (error) {
            console.log('ERROR TRYING TO DO THE FIRESTORE QUERY IN TOTAL');
            return JSON.stringify(error);
        }
    },

}); 