import { http } from "./http";

export type CreateCustomerRequest = {
  customerType: "Individual" | "Company";
  name: string;
  cpfCnpj: string;
  email: string;
  phone: string | null;
  birthOrFoundationDate: string;
  stateRegistration: string | null;
  isStateRegistrationExempt: boolean;

  postalCode: string | null;
  street: string | null;
  number: string | null;
  neighborhood: string | null;
  city: string | null;
  state: string | null;

  performedBy: string;
};

export type CreateCustomerResponse = { id: string };

export async function createCustomer(payload: CreateCustomerRequest): Promise<CreateCustomerResponse> {
  const res = await http.post<CreateCustomerResponse>("/customers", payload);

  return res.data;
}

export type CustomerReadModel = {
  id: string;
  customerType: string;
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

export async function getCustomerById(id: string): Promise<CustomerReadModel> {
  const res = await http.get<CustomerReadModel>(`/customers/${encodeURIComponent(id)}`);

  return res.data;
}