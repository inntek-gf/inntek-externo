import { Dropdown, Popover, Tooltip, Tab } from "bootstrap";
import SweetAlert from 'sweetalert2';

/** 
 * Implementación de las utilerias de los componentes de Bootstrap. 
 * @author Fco. Eduardo.
 * @version 1.0.0
 */
class BootstrapUtilities {

    /** 
    * Habilita la la vizualización de los componentes Tooltip.
    * @description 11/04/2023 11:01:50 a. m.
    * @author Fco. Eduardo.
    * @since 1.0.0
    */
    public static async enableTooltipsAsync(): Promise<void> {
        const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]');
        if (tooltipTriggerList.length !== 0) {
            tooltipTriggerList.forEach(tooltipTriggerEl => new Tooltip(tooltipTriggerEl));
        }
    }

    /**
    * Habilita la la vizualización de los componentes Popover.
    * @description 11/04/2023 11:01:50 a. m.
    * @author Fco. Eduardo.
    * @since 1.0.0
    */
    public static async enablePopoversAsync(): Promise<void> {
        const popoverTriggerList = document.querySelectorAll('[data-bs-toggle="popover"]');
        if (popoverTriggerList.length !== 0) {
            popoverTriggerList.forEach(popoverTriggerEl => new Popover(popoverTriggerEl));
        }
    }

    /**
    * Habilita la la vizualización de los componentes Dropdown.
    * @description 11/04/2023 11:01:50 a. m.
    * @author Fco. Eduardo.
    * @since 1.0.0
    */
    public static async enableDropdownsAsync(): Promise<void> {
        const triggerList = document.querySelectorAll('.dropdown-toggle');
        if (triggerList.length !== 0) {
            triggerList.forEach(dropdownNode => new Dropdown(dropdownNode));
        }
    }

    /**
    * Habilita la la vizualización de los componentes Tab.
    * @description 24/05/2023 01:08:50 p. m.
    * @author Fco. Eduardo.
    * @since 1.0.0
    */
    public static async enableTabsAsync(): Promise<void> {
        const triggerTabList = document.querySelectorAll('#myTab button');
        if (triggerTabList.length !== 0) {
            triggerTabList.forEach(triggerEl => {
                const tabTrigger = new Tab(triggerEl);
                triggerEl.addEventListener('click', event => {
                    event.preventDefault()
                    tabTrigger.show()
                });
            });
        }
    }
}

async function SetAllConfigurations(): Promise<any> {

    await Promise.allSettled([
        BootstrapUtilities.enableDropdownsAsync(),
        BootstrapUtilities.enablePopoversAsync(),
        BootstrapUtilities.enableTooltipsAsync(),
        BootstrapUtilities.enableTabsAsync()
    ]);

    await SweetAlert.fire({
        titleText: "Demo",
        text: "Mensaje de prueba de alerta.",
        icon: "info",
        confirmButtonText: "Continuar",
        buttonsStyling: false,
        customClass: {
            confirmButton: 'btn btn-primary shadow w-100',
        }
    });
}

window.addEventListener('load', SetAllConfigurations);