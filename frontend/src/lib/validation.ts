import * as yup from "yup";
import { isValid as isValidCpf } from "@fnando/cpf";
import { isValid as isValidCnpj } from "@fnando/cnpj";

import type { CustomerType, CreateCustomerRequest } from "./types";

const onlyDigits = (value: string) => (value ?? "").replace(/\D/g, "");

export const createCustomerSchema: yup.ObjectSchema<CreateCustomerRequest> = yup
  .object({
    customerType: yup
      .mixed<CustomerType>()
      .oneOf(["Individual", "Company"])
      .required("Tipo do cliente é obrigatório"),

    performedBy: yup.string().trim().required("Campo 'Realizado por' é obrigatório"),

    name: yup.string().trim().required("Nome/Razão social é obrigatório"),

    cpfCnpj: yup
      .string()
      .trim()
      .required("CPF/CNPJ é obrigatório")
      .test("cpf-cnpj", "CPF/CNPJ inválido", function (value) {
        const { customerType } = this.parent as CreateCustomerRequest;

        const digits = onlyDigits(value ?? "");

        if (customerType === "Company") {
          return isValidCnpj(digits);
        }

        return isValidCpf(digits);
      }),

    email: yup.string().trim().email("E-mail inválido").required("E-mail é obrigatório"),

    phone: yup
      .string()
      .nullable()
      .transform((value) => (value?.trim() ? value : null))
      .optional(),

    birthOrFoundationDate: yup
      .string()
      .required("Data é obrigatória")
      .test("date-format", "Data inválida", (value) => {
        if (!value) {
          return false;
        }
        
        return /^\d{4}-\d{2}-\d{2}$/.test(value);
      }),

    stateRegistration: yup
      .string()
      .nullable()
      .transform((value) => (value?.trim() ? value : null))
      .when(["customerType", "isStateRegistrationExempt"], {
        is: (type: CustomerType, exempt: boolean) => type === "Company" && !exempt,
        then: (schema) => schema.required("Inscrição estadual é obrigatória para PJ (ou marque ISENTO)"),
        otherwise: (schema) => schema.optional(),
      }),

    isStateRegistrationExempt: yup.boolean().required(),

    postalCode: yup
      .string()
      .nullable()
      .transform((value) => (value?.trim() ? value : null))
      .test("cep", "CEP deve ter 8 dígitos", (value) => {
        if (!value) {
          return true;
        }

        return onlyDigits(value).length === 8;
      }),

    street: yup.string().nullable().transform((value) => (value?.trim() ? value : null)).optional(),
    number: yup.string().nullable().transform((value) => (value?.trim() ? value : null)).optional(),
    neighborhood: yup.string().nullable().transform((value) => (value?.trim() ? value : null)).optional(),
    city: yup.string().nullable().transform((value) => (value?.trim() ? value : null)).optional(),
    state: yup.string().nullable().transform((value) => (value?.trim() ? value : null)).optional(),
  })
  .required();