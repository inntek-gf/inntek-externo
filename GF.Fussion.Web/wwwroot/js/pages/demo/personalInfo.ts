import { PersonalInfoApiRepository } from '../../repository/demo-repository.js';

let gridApi: any;

//#region Repositorio.

//#endregion

//#region Formulario.
function getForm(): HTMLFormElement {
    const form: HTMLFormElement = document.forms["personalInfoForm"];
    return form;
}
function getFormData(): any {

    const form: HTMLFormElement = getForm();

    const formData = {
        firstname: form.elements["name"].value,
        lastname: form.elements["lastname"].value,
        age: form.elements["age"].value
    };

    return formData;
}
function setFormData(request: any) {

    const form = getForm();

    if (request.firstname) {
        form["firstname"].value = request.firstname;
    }

    if (request.lastname) {
        form["lastname"].value = request.lastname;
    }

    if (request.age) {
        form["age"].value = request.age;
    }
}
//#endregion

//#region Private Methods.
async function GetAllRecords() {

    const repository = new PersonalInfoApiRepository();
    gridApi.setGridOption("rowData", await repository.getAll());
}
function handleUpdate(rowData) {

    setFormData({
        firstname: rowData.firstname,
        lastname: rowData.lastname,
        age: rowData.age
    });
}
async function AddOrUpdate(event) {
    event.preventDefault();

    const form = getForm();
    const personalInfo = getFormData();
    let messageAlert: string | null = null;

    const repository = new PersonalInfoApiRepository();
    alert(personalInfo.id)

    if (personalInfo.id === 0) {
        alert('Add')
        repository.add(personalInfo);
        messageAlert = "Information has been added";
    }
    else {
        alert('Update')
        repository.update(personalInfo);
        messageAlert = "Information has been updated";
    }

    alert(messageAlert);
    GetAllRecords();
    form.reset();
}
//#endregion

//#region Document Events.
document.addEventListener('DOMContentLoaded', async () => {

    const gridOptions = {
        columnDefs: [
            { field: "id" },
            { field: "fullName" },
            { field: "age" },
            {
                headerName: "Actions",
                field: "actions",
                cellRenderer: function (params) {
                    const updateButton = document.createElement("button");
                    updateButton.textContent = "Update";
                    updateButton.classList.add("btn");
                    updateButton.classList.add("btn-warning");
                    updateButton.classList.add("btn-sm");
                    updateButton.classList.add("me-3");

                    updateButton.value = params.data.id;

                    updateButton.addEventListener("click", () => {
                        handleUpdate(params.data);
                    });

                    const deleteButton = document.createElement("button");
                    deleteButton.textContent = "Delete";
                    deleteButton.classList.add("btn");
                    deleteButton.classList.add("btn-danger");
                    deleteButton.classList.add("btn-sm");
                    deleteButton.value = params.data.id;

                    deleteButton.addEventListener("click", () => {
                        //handleDelete(params.data);
                    });

                    const buttonContainer = document.createElement("div");
                    buttonContainer.classList.add("action-buttons");
                    buttonContainer.appendChild(updateButton);
                    buttonContainer.appendChild(deleteButton);

                    return buttonContainer;
                }
            }
        ],
    };

    //@ts-ignore
    gridApi = agGrid.createGrid(document.querySelector("#demoTable"), gridOptions);

    const form = getForm();
    form.onsubmit = AddOrUpdate;

    await GetAllRecords();

});
//#endregion