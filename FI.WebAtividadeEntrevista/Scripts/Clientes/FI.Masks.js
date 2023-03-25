$(document).ready(function () {
    let cpfs = document.querySelectorAll('[data-mask-type="cpf"]');

    for (let cpf of cpfs) {
        IMask(cpf, {
            mask: '000.000.000-00'
        });
    }
});