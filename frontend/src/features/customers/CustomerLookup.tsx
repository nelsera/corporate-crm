import { useState } from "react";
import { Card } from "../../components/ui/Card";
import { Field } from "../../components/ui/Field";
import { Input } from "../../components/ui/Input";
import { Button } from "../../components/ui/Button";
import { getCustomerById, type CustomerReadModel } from "../../services/customers";

export function CustomerLookup(props: { defaultId?: string }) {
  const [id, setId] = useState(props.defaultId ?? "");

  const [loading, setLoading] = useState(false);

  const [error, setError] = useState<string | null>(null);

  const [customer, setCustomer] = useState<CustomerReadModel | null>(null);

  async function handleGet() {
    setError(null);

    setCustomer(null);

    if (!id.trim()) {
      setError("Provide a customer id.");

      return;
    }

    setLoading(true);

    try {
      const data = await getCustomerById(id.trim());

      setCustomer(data);
    } catch (e: any) {
      setError(e?.message ?? "Query failed");
    } finally {
      setLoading(false);
    }
  }

  return (
    <Card title="Get customer" description="Fetch customer read model by id.">
      <div className="grid gap-3">
        <Field label="Customer id" hint="Cole um id (uuid)">
          <div className="flex gap-2">
            <Input value={id} onChange={(e) => setId(e.target.value)} className="flex-1" />
            <Button onClick={handleGet} disabled={loading} variant="ghost">
              {loading ? "Loading..." : "Get"}
            </Button>
          </div>
        </Field>

        {error && <p className="text-sm text-rose-400">{error}</p>}

        {customer && (
          <pre className="max-h-[520px] overflow-auto rounded-2xl border border-white/10 bg-black/30 p-4 text-xs text-slate-200">
            {JSON.stringify(customer, null, 2)}
          </pre>
        )}
      </div>
    </Card>
  );
}