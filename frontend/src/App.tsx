import { useState } from "react";
import { Page } from "./components/ui/Page";
import { CustomerCreateForm } from "./features/customers/CustomerCreateForm";
import { CustomerLookup } from "./features/customers/CustomerLookup";

export default function App() {
  const [lastCreatedId, setLastCreatedId] = useState<string | undefined>(undefined);

  return (
    <Page
      title="Corporate CRM"
    >
      <div className="grid grid-cols-1 gap-8 lg:grid-cols-2">
        <CustomerCreateForm onCreated={(id) => setLastCreatedId(id)} />
        <CustomerLookup defaultId={lastCreatedId} />
      </div>

    </Page>
  );
}
