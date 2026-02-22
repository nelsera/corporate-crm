export type CustomerType = "Individual" | "Company";

export type CreateCustomerRequest = {
  customerType: CustomerType;
  performedBy: string;

  name: string;
  cpfCnpj: string;
  email: string;
  phone?: string | null;

  birthOrFoundationDate: string;
  stateRegistration?: string | null;
  isStateRegistrationExempt: boolean;

  postalCode?: string | null;
  street?: string | null;
  number?: string | null;
  neighborhood?: string | null;
  city?: string | null;
  state?: string | null;
};

export type CreateCustomerResponse = {
  id: string;
};

export type PostalCodeLookupResponse = {
  cep: string;
  street: string | null;
  neighborhood: string | null;
  city: string | null;
  state: string | null;
};

export type CustomerReadModel = {
  id: string;
  customerType: CustomerType;
  name: string;
  cpfCnpj: string;
  email: string;
  phone?: string | null;
  birthOrFoundationDate: string;
  stateRegistration?: string | null;
  isStateRegistrationExempt: boolean;
  postalCode?: string | null;
  street?: string | null;
  number?: string | null;
  neighborhood?: string | null;
  city?: string | null;
  state?: string | null;
  createdAt: string;
  updatedAt: string;
};