import { useMemo, useState } from "react";
import { useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import axios from "axios";

import { createCustomerSchema } from "../../lib/validation";
import { http } from "../../services/http";
import { extractApiMessage, extractApiValidationErrors } from "../../lib/apiErrors";

import { Card } from "../../components/ui/Card";
import { Input } from "../../components/ui/Input";
import { Select } from "../../components/ui/Select";
import { Button } from "../../components/ui/Button";
import { FieldErrorText } from "../../components/ui/FieldErrorText";
import { SuccessModal } from "../../components/ui/SuccessModal";

type FormValues = {
  customerType: "Individual" | "Company";
  performedBy: string;

  name: string;
  cpfCnpj: string;
  email: string;
  phone?: string;

  birthOrFoundationDate: string;
  stateRegistration?: string;
  isStateRegistrationExempt: boolean;

  postalCode?: string;
  number?: string;

  street?: string;
  neighborhood?: string;
  city?: string;
  state?: string;
};

export function CustomerCreateForm({
  onCreated,
}: {
  onCreated?: (id: string) => void;
}) {
  const [submitError, setSubmitError] = useState<string | null>(null);

  const [cepHint, setCepHint] = useState<string | null>(null);

  const [success, setSuccess] = useState<null | { id?: string; name?: string }>(null);

  const [isCepLoading, setIsCepLoading] = useState(false);

  const [cepStatus, setCepStatus] = useState<string | null>(null);

  const defaultValues = useMemo<FormValues>(
    () => ({
      customerType: "Individual",
      performedBy: "nelson",
      name: "",
      cpfCnpj: "",
      email: "",
      phone: "",
      birthOrFoundationDate: "",
      stateRegistration: "",
      isStateRegistrationExempt: false,
      postalCode: "",
      number: "",
      street: "",
      neighborhood: "",
      city: "",
      state: "",
    }),
    []
  );

  const {
    register,
    handleSubmit,
    setValue,
    setError,
    clearErrors,
    watch,
    formState: { errors, isSubmitting },
    reset,
  } = useForm<FormValues>({
    defaultValues,
    resolver: yupResolver(createCustomerSchema),
    mode: "onBlur",
  });

  const customerType = watch("customerType");

  async function lookupCepIfPossible(raw: string) {
    const digits = (raw || "").replace(/\D/g, "");

    if (digits.length !== 8) {
      return;
    }

    try {
      setIsCepLoading(true);
     
      setCepHint("Buscando CEP...");
     
      const res = await http.get(`/postal-codes/${digits}`);
     
      setValue("street", res.data.street ?? "");
      setValue("neighborhood", res.data.neighborhood ?? "");
      setValue("city", res.data.city ?? "");
      setValue("state", res.data.state ?? "");
      setCepHint(null);
    } catch {
      setCepHint("Não foi possível buscar o CEP.");
    } finally {
      setIsCepLoading(false);
     
      setTimeout(() => setCepStatus(null), 2500);
    }
  }

  async function onSubmit(values: FormValues) {
    setSubmitError(null);

    try {
      clearErrors();

      const payload = {
        customerType: values.customerType,
        name: values.name,
        cpfCnpj: values.cpfCnpj,
        email: values.email,
        phone: values.phone || null,
        birthOrFoundationDate: values.birthOrFoundationDate,
        stateRegistration: values.stateRegistration || null,
        isStateRegistrationExempt: values.isStateRegistrationExempt,
        postalCode: values.postalCode || null,
        street: values.street || null,
        number: values.number || null,
        neighborhood: values.neighborhood || null,
        city: values.city || null,
        state: values.state || null,
        performedBy: values.performedBy,
      };

      const res = await http.post("/customers", payload);

      const createdId = res.data?.id ?? res.data?.customerId;

      const createdName = payload?.name;

      setSuccess({ id: createdId, name: createdName });

      if (createdId && onCreated) {
        onCreated(createdId);
      }

      setSubmitError(null);
    } catch (err) {
      if (axios.isAxiosError(err)) {
        const data = err.response?.data;
       
        const fieldErrors = extractApiValidationErrors(data);

        if (fieldErrors.length > 0) {
          fieldErrors.forEach((fe) => {
            setError(fe.field as any, { type: "server", message: fe.message });
          });

          setSubmitError(fieldErrors[0].message);
       
          return;
        }

        const apiMsg = extractApiMessage(data);

        if (apiMsg) {
          setSubmitError(apiMsg);
          return;
        }

        setSubmitError(`Erro ao criar cliente (HTTP ${err.response?.status ?? "?"}).`);

        return;
      }

      setSubmitError("Erro inesperado ao criar cliente.");
    }
  }

  return (
    <>
      <Card title="Criar cliente" subtitle="Cria um cliente e preenche o endereço automaticamente pelo CEP (API → ViaCEP).">
        <form className="space-y-6" onSubmit={handleSubmit(onSubmit)}>
          <div className="grid gap-4 md:grid-cols-2">
            <div>
              <label className="text-sm font-medium text-slate-700">Tipo de cliente</label>

              <Select {...register("customerType")} className="mt-1">
                <option value="Individual">Pessoa física</option>
                <option value="Company">Pessoa jurídica</option>
              </Select>

              <FieldErrorText message={errors.customerType?.message} />
            </div>

            <div>
              <label className="text-sm font-medium text-slate-700">Realizado por</label>

              <Input className="mt-1" {...register("performedBy")} placeholder="ex: nelson" />

              <p className="mt-1 text-xs text-slate-500">Usado como auditoria</p>

              <FieldErrorText message={errors.performedBy?.message} />
            </div>

            <div>
              <label className="text-sm font-medium text-slate-700">
                {customerType === "Company" ? "Razão social" : "Nome"}
              </label>

              <Input className="mt-1" {...register("name")} />

              <FieldErrorText message={errors.name?.message} />
            </div>

            <div>
              <label className="text-sm font-medium text-slate-700">
                {customerType === "Company" ? "CNPJ" : "CPF"}
              </label>

              <Input className="mt-1" {...register("cpfCnpj")} />

              <FieldErrorText message={errors.cpfCnpj?.message} />
            </div>

            <div>
              <label className="text-sm font-medium text-slate-700">E-mail</label>

              <Input className="mt-1" {...register("email")} />

              <FieldErrorText message={errors.email?.message} />
            </div>

            <div>
              <label className="text-sm font-medium text-slate-700">Telefone</label>

              <Input className="mt-1" {...register("phone")} placeholder="Opcional" />

              <FieldErrorText message={errors.phone?.message} />
            </div>

            <div>
              <label className="text-sm font-medium text-slate-700">
                {customerType === "Company" ? "Data de fundação" : "Data de nascimento"}
              </label>

              <Input className="mt-1" type="date" lang="pt-BR" {...register("birthOrFoundationDate")} />

              <FieldErrorText message={errors.birthOrFoundationDate?.message} />
            </div>

            <div>
              <label className="text-sm font-medium text-slate-700">Inscrição estadual</label>

              <Input className="mt-1" {...register("stateRegistration")} placeholder="Opcional" />

              <FieldErrorText message={errors.stateRegistration?.message} />
            </div>

            <div className="md:col-span-2">
              <label className="inline-flex items-center gap-2 text-sm text-slate-700">
                <input
                  type="checkbox"
                  className="h-4 w-4"
                  {...register("isStateRegistrationExempt")}
                />
                ISENTO
              </label>

              <FieldErrorText message={errors.isStateRegistrationExempt?.message as any} />
            </div>
          </div>

          <div className="border-t pt-6">
            <h3 className="text-sm font-semibold text-slate-900">Endereço</h3>

            <p className="mt-1 text-xs text-slate-500">
              Digite o CEP e saia do campo para buscar automaticamente.
            </p>

            <div className="mt-4 grid gap-4 md:grid-cols-2">
              <div>
                <label className="text-sm font-medium text-slate-700">CEP</label>

                <div className="relative mt-1">
                  <input
                    {...register("postalCode")}
                    onBlur={(e) => lookupCepIfPossible(e.target.value)}
                    className="w-full rounded-lg border border-slate-200 bg-white px-3 py-2 pr-10 text-slate-900 shadow-sm outline-none focus:border-blue-500 focus:ring-4 focus:ring-blue-100"
                    placeholder="00000-000"
                  />

                  {isCepLoading && (
                    <div className="pointer-events-none absolute inset-y-0 right-3 flex items-center">
                      <span className="h-4 w-4 animate-spin rounded-full border-2 border-slate-300 border-t-slate-700" />
                    </div>
                  )}
                </div>

                {cepStatus && (
                  <p className="mt-1 text-xs text-slate-500">{cepStatus}</p>
                )}

                {cepHint ? <p className="mt-1 text-xs text-slate-500">{cepHint}</p> : null}

                <FieldErrorText message={errors.postalCode?.message} />
              </div>

              <div>
                <label className="text-sm font-medium text-slate-700">Número</label>

                <Input className="mt-1" {...register("number")} />

                <FieldErrorText message={errors.number?.message} />
              </div>

              <div>
                <label className="text-sm font-medium text-slate-700">Rua</label>

                <Input className="mt-1" {...register("street")} />

                <FieldErrorText message={errors.street?.message} />
              </div>

              <div>
                <label className="text-sm font-medium text-slate-700">Bairro</label>

                <Input className="mt-1" {...register("neighborhood")} />

                <FieldErrorText message={errors.neighborhood?.message} />
              </div>

              <div>
                <label className="text-sm font-medium text-slate-700">Cidade</label>

                <Input className="mt-1" {...register("city")} />

                <FieldErrorText message={errors.city?.message} />
              </div>

              <div>
                <label className="text-sm font-medium text-slate-700">UF</label>

                <Input className="mt-1" {...register("state")} />

                <FieldErrorText message={errors.state?.message} />
              </div>
            </div>
          </div>

          {submitError ? (
            <div className="rounded-lg border border-rose-200 bg-rose-50 px-4 py-3 text-sm text-rose-700">
              {submitError}
            </div>
          ) : null}

          <div className="flex items-center gap-3">
            <Button type="submit" disabled={isSubmitting || isCepLoading}>
              {isSubmitting ? "Criando..." : "Criar cliente"}
            </Button>
          </div>
        </form>
      </Card>

      <SuccessModal
        open={!!success}
        title="Cliente cadastrado com sucesso!"
        description="Você já pode consultar o cliente pelo ID."
        onClose={() => setSuccess(null)}
        onPrimary={() => {
          reset();

          setSubmitError(null);

          setSuccess(null);
        }}
        primaryText="Cadastrar outro"
        secondaryText="Fechar"
      >
        <div className="rounded-xl border border-slate-200 bg-slate-50 p-3">
          <div className="text-sm text-slate-600">Cliente</div>

          <div className="font-semibold text-slate-900">{success?.name ?? "-"}</div>

          <div className="mt-3 text-sm text-slate-600">ID</div>

          <div className="flex items-center gap-2">
            <code className="flex-1 truncate rounded-lg bg-white px-2 py-1 text-xs text-slate-800 ring-1 ring-slate-200">
              {success?.id ?? "—"}
            </code>

            <button
              type="button"
              onClick={async () => {
                if (!success?.id) {
                  return;
                }

                await navigator.clipboard.writeText(success.id);
              }}
              className="rounded-lg border border-slate-200 bg-white px-3 py-2 text-xs font-semibold text-slate-700 hover:bg-slate-50"
            >
              Copiar
            </button>
          </div>
        </div>
      </SuccessModal>
    </>
  );
}
