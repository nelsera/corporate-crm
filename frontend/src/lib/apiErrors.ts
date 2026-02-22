export type ApiErrorItem = {
  propertyName?: string;
  errorMessage?: string;
  PropertyName?: string;
  ErrorMessage?: string;
};

export function normalizeFieldName(apiField: string) {
  const map: Record<string, string> = {
    CustomerType: "customerType",
    Name: "name",
    CpfCnpj: "cpfCnpj",
    Email: "email",
    Phone: "phone",
    BirthOrFoundationDate: "birthOrFoundationDate",
    StateRegistration: "stateRegistration",
    IsStateRegistrationExempt: "isStateRegistrationExempt",
    PostalCode: "postalCode",
    Street: "street",
    Number: "number",
    Neighborhood: "neighborhood",
    City: "city",
    State: "state",
    PerformedBy: "performedBy",
  };

  return map[apiField] ?? apiField;
}

export function extractApiMessage(data: any): string | null {
  if (!data) {
    return null;
  }

  return data.message;
}

export function extractApiValidationErrors(data: any) {
  const list = Array.isArray(data?.errors) ? (data.errors as ApiErrorItem[]) : [];

  const mapped = list
    .map((errors) => {
      const propertyName = errors.propertyName ?? errors.PropertyName;

      const errorMessage = errors.errorMessage ?? errors.ErrorMessage;

      if (!propertyName || !errorMessage) {
        return null;
      }

      return {
        field: normalizeFieldName(propertyName),
        message: errorMessage,
      };
    })
    .filter(Boolean) as Array<{ field: string; message: string }>;

  return mapped;
}