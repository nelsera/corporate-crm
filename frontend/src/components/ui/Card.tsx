export function Card(props: { title: string; description?: string; children: React.ReactNode }) {
  return (
    <section className="rounded-2xl bg-white p-6 shadow-sm ring-1 ring-slate-200">
      <div className="mb-6">
        <h2 className="text-lg font-semibold text-slate-900">
          {props.title}
        </h2>

        {props.description && (
          <p className="mt-1 text-sm text-slate-500">
            {props.description}
          </p>
        )}
      </div>

      {props.children}
    </section>
  );
}