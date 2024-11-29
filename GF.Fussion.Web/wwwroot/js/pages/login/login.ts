const form: HTMLFormElement = document.getElementById('formulario') as HTMLFormElement;

form.addEventListener('login', async function (event: any) {

    const response: any = event.detail;

    if (response && response.key != null) {

        const data: any = {
            Token: response.key,
            DeviceInfo: {
                Platform: "",
                Vendor: "",
                UserAgent: navigator.userAgent,
                AppVersion: "",
                ScreenWidth: window.screen.availWidth.toString(),
                ScreenHeight: window.screen.availHeight.toString()
            }
        };

        console.log(data);
        //try {
        //    await repository.Users.RequestSession(data);
        //    window.location.href = '/Index';
        //}
        //catch (e) {
        //    console.error(e);
        //}
    }
});