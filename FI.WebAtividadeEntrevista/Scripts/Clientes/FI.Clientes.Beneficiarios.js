$(document).ready(function () {

    renderBeneficiarios();

    $('#FormularioBeneficiario').submit(function (e) {
        e.preventDefault();

        let index = parseInt($('#FormularioBeneficiario [name=IndexBeneficiario]').val());
        let cpf = $('#FormularioBeneficiario [name=CPFBeneficiario]').val();
        let nome = $('#FormularioBeneficiario [name=NomeBeneficiario]').val();

        if (
            Number.isNaN(index) &&
            beneficiarios.reduce((_, val) => val.CPF === cpf ? val.CPF : null, null)
        ) {
            ModalDialog('', 'CPF já cadastrado');
            return;
        }

        $.get(`/Cliente/IsCPFValid?cpf=${cpf}`).done(function (result) {
            if (!result) {
                if (index >= 0) {
                    beneficiarios[index] = {
                        Id: beneficiarios[index].Id,
                        CPF: cpf,
                        Nome: nome,
                    };
                }
                else {
                    beneficiarios.push({
                        Id: 0,
                        CPF: cpf,
                        Nome: nome,
                    });
                }

                $('#FormularioBeneficiario [name=IndexBeneficiario]').val('');
                $('#FormularioBeneficiario [name=CPFBeneficiario]').val('');
                $('#FormularioBeneficiario [name=NomeBeneficiario]').val('');

                renderBeneficiarios();
            }
            else {
                ModalDialog('', result);
            }
        });
    })
});

function alterarBeneficiario(index) {
    let beneficiario = beneficiarios[index];

    $('#FormularioBeneficiario [name=IndexBeneficiario]').val(index);
    $('#FormularioBeneficiario [name=CPFBeneficiario]').val(beneficiario.CPF);
    $('#FormularioBeneficiario [name=NomeBeneficiario]').val(beneficiario.Nome);
}

function excluirBeneficiario(index) {
    beneficiarios.splice(index, 1);
    renderBeneficiarios();
}

function renderBeneficiarios() {
    if (!beneficiarios) return;

    let tableBody = $('#TableBeneficiarios tbody');
    tableBody.html('');

    let container = $("#InputBeneficiariosContainer");
    container.html('');

    for (let i = 0; i < beneficiarios.length; i++) {

        tableBody.append(`
            <tr>
                <td>${beneficiarios[i].CPF}</td>
                <td>${beneficiarios[i].Nome}</td>
                <td>
                    <button type="button" class="btn btn-primary mr-2" onclick="alterarBeneficiario(${i})">Alterar</button>
                    <button type="button" class="btn btn-primary" onclick="excluirBeneficiario(${i})">Excluir</button>
                </td>
            </tr>
        `);

        container.append(`
            <input type="hidden" name="Beneficiarios[${i}].Id" value="${beneficiarios[i].Id}" />
            <input type="hidden" name="Beneficiarios[${i}].CPF" value="${beneficiarios[i].CPF}" />
            <input type="hidden" name="Beneficiarios[${i}].Nome" value="${beneficiarios[i].Nome}" />
        `);
    }
}

