import { http } from "./http";

export type PostalCodeResponse = {
  cep: string;
  street: string | null;
  neighborhood: string | null;
  city: string | null;
  state: string | null;
};

export async function lookupPostalCode(cep: string): Promise<PostalCodeResponse> {
  const res = await http.get<PostalCodeResponse>(`/postal-codes/${encodeURIComponent(cep)}`);

  return res.data;
}