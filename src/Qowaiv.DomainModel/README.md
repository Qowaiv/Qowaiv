# Qowaiv.DomainModel
Qowaiv Domain Model is a library containing the (abstract) building blocks to
set up a Domain-Driven application.

## Event Sourcing
Within Qowaiv Domain Model, the choice has been made to only support DDD via
Event Sourcing. In short: Event Sourcing describes the state of an aggregate
(root) by the (domain) events that occurred within the domain. Getting the
current state of an aggregate can always be achieved by replaying these
events. 

## Always Valid
The aggregate should always be valid according to the boundaries of its domain.
There are multiple ways to achieve this, but within Qowaiv Domain Model this is
guaranteed via an implicitly triggered validator.

When a public method is called that would lead to a new aggregate state, the
events describing the change are only added to the event buffer
associated with the aggregate if the new state is valid according to the
rules specified in the validator.

## Aggregate
An aggregate is a cluster of associated objects that we treat as a unit for the
purpose of data changes. When implementing an aggregate there are several steps
that have to be taken.

## Further reading
More info can be found at https://github.com/Qowaiv/qowaiv-domainmodel.
