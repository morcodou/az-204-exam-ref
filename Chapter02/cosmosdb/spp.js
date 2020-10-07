// Click the New Stored Procedure button. This button creates a new sample stored procedure that you can use as a template for your stored procedures.

// In the Stored Procedure Id text box, provide a name for the stored procedure.

// Replace the content of the New Stored Procedure tab with the content of Listing 2-6.

// Listing 2-6 Cosmos DB SQL API stored procedure

// Click here to view code image

//JavaScript
function createNewDocument(docToCreate) {
    var context = getContext();
    var container = context.getCollection();
    var response = context.getResponse();

    console.log(docToCreate);
    var accepted = container.createDocument(container.getSelfLink(),
        docToCreate,
        function (err, docCreated) {
            if (err) throw new Error('Error creating a new document: ' + err.message);
            response.setBody(docCreated);
        });

    if (!accepted) return;
}