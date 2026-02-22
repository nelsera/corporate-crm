export type CustomerType = "Individual" | "Company";

export type CustomerFormState = {
  customerType: CustomerType;
  name: string;
  cpfCnpj: string;
  email: string;
  phone: string;
  birthOrFoundationDate: string;

  stateRegistration: string;
  isStateRegistrationExempt: boolean;

  postalCode: string;
  street: string;
  number: string;
  neighborhood: string;
  city: string;
  state: string;

  performedBy: string;
};